using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using DbUp;
using DbUp.ScriptProviders;
using Npgsql;

namespace Infrastructure.Manager.Scripts;

internal class ScriptRunner
{
    private readonly IConfiguration _configuration;

    private static readonly Regex AppRootPathMatcher = new(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");

    public ScriptRunner(IConfiguration configuration) => _configuration = configuration;

    public void Run(bool dropDatabase)
    {
        if (dropDatabase)
            DropDatabase();

        RunScripts("Scripts/runAfter/");
        RunScripts("Scripts/tables/");
        RunScripts("Scripts/indexes/");
        RunScripts("Scripts/functions/");
        RunScripts("Scripts/triggers/");
        RunScripts("Scripts/procedures/");
        RunScripts("Scripts/runBefore/");
    }

    private void DropDatabase()
    {
        NpgsqlConnectionStringBuilder builder = new(_configuration.GetConnectionString("ConnSql"));
        var databaseName = builder.Database;
        builder.Database = builder.Username;

        using NpgsqlConnection connection = new(builder.ToString());
        connection.Open();

        using var command =
            new NpgsqlCommand(
                $"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = \'{databaseName}\'; DROP DATABASE IF EXISTS \"{databaseName}\"");

        command.ExecuteNonQuery();
        connection.Close();
    }

    private void RunScripts(string scriptPath)
    {
        var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var rootPath = AppRootPathMatcher.Match(exePath).Value;

        var upgradeEngine = DeployChanges
            .To
            .PostgresqlDatabase(_configuration.GetConnectionString("ConnSql"))
            .WithScriptsFromFileSystem
            (
                Path.Combine(rootPath, scriptPath), new FileSystemScriptOptions
                {
                    IncludeSubDirectories = true
                }
            )
            .WithTransactionPerScript()
            .WithVariablesDisabled()
            .Build();

        var upgrader = upgradeEngine.PerformUpgrade();

        if (!upgrader.Successful)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(upgrader.Error);
            Console.ResetColor();
            Console.ReadLine();
        }
        else
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
    }
}
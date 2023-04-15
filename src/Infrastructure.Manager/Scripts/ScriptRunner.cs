using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using DbUp;
using DbUp.ScriptProviders;

namespace Infrastructure.Manager.Scripts;

internal class ScriptRunner
{
    private readonly IConfiguration _configuration;

    private static readonly Regex AppRootPathMatcher = new(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");

    public ScriptRunner(IConfiguration configuration) => _configuration = configuration;

    public void Run(bool dropDatabase)
    {
        if (dropDatabase)
            DropDatabase.For.SqlDatabase(_configuration.GetConnectionString("ConnSql"));

        RunScripts("Scripts/runAfter/");
        RunScripts("Scripts/tables/");
        RunScripts("Scripts/indexes/");
        RunScripts("Scripts/functions/");
        RunScripts("Scripts/triggers/");
        RunScripts("Scripts/procedures/");
        RunScripts("Scripts/runBefore/");
    }

    private void RunScripts(string scriptPath)
    {
        var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var rootPath = AppRootPathMatcher.Match(exePath).Value;

        var upgradeEngine = DeployChanges
            .To
            .SqlDatabase(_configuration.GetConnectionString("ConnSql"))
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
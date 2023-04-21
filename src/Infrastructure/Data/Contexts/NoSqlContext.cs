using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Domain.Core.Entities;
using Infrastructure.Data.Mappings;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Infrastructure.Data.Contexts;

public class NoSqlContext : IDisposable
{
    public IMongoDatabase Database { get; set; }
    public MongoClient MongoClient { get; set; }
    readonly List<Func<Task>> _commands;
    readonly IConfiguration _configuration;
    readonly ILogger<NoSqlContext> _logger;

    public NoSqlContext(IConfiguration configuration, ILogger<NoSqlContext> logger)
    {
        _configuration = configuration;
        _commands = new List<Func<Task>>();
        _logger = logger;
    }

    public IMongoCollection<Audit> Audits
        => GetCollection<Audit>(typeof(Audit).Name);

    public async Task<int> SaveChanges()
    {
        try
        {
            ConfigureMongo();

            var commandTasks = _commands.Select(c => c());

            await Task.WhenAll(commandTasks);

            return _commands.Count;
        }
        catch (Exception e)
        {
            _logger.LogError(string.Format("NoSqlContext - {0} - {1}", e.Message, e.StackTrace.ToString()));
        }

        return _commands.Count;
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        ConfigureMongo();

        return Database.GetCollection<TEntity>(name);
    }

    public void AddCommand(Func<Task> func)
        => _commands.Add(func);

    public void Dispose()
        => GC.SuppressFinalize(this);

    private void ConfigureMongo()
    {
        if (MongoClient != null)
            return;

        MongoClient = new MongoClient(_configuration.GetConnectionString("ConnNoSql"));
        Database = MongoClient.GetDatabase(new MongoUrl(_configuration.GetConnectionString("ConnNoSql")).DatabaseName);
    }
}

public static class MongoDbPersistence
{
    public static void Configure()
    {
        AuditMapping.Configure();

        var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
        ConventionRegistry.Register("My Solution Conventions", pack, t => true);
    }
}
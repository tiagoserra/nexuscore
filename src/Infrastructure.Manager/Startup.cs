using System.Reflection;
using Application;
using Application.Common.Interfaces;
using Infrastructure.Manager.Consts;
using Infrastructure.Manager.Enums;
using Infrastructure.Manager.Interfaces;
using Infrastructure.Manager.Scripts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Manager;

public class Startup
{
    private readonly IServiceCollection _serviceCollection;
    private readonly IConfiguration _configuration;
    private readonly EnvironmentDumpActionManager _action;
    private readonly ScriptRunner _scriptRunner;
    private ServiceProvider _serviceProvider;

    public Startup(EnvironmentDumpManager environmentType, EnvironmentDumpActionManager action)
    {
        var environment = environmentType switch
        {
            EnvironmentDumpManager.Development => "",
            EnvironmentDumpManager.Staging => "Staging",
            EnvironmentDumpManager.Production => "Production",
            _ => ""
        };

        _serviceCollection = new ServiceCollection();

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();
        _serviceCollection.AddSingleton<IConfiguration>(provider => _configuration);
        _scriptRunner = new ScriptRunner(_configuration);
        _action = action;

        ConsoleDraw.DrawEnvironment(environment is "" ? "local" : environment);
    }

    public async Task RunAsync()
    {
        if (_action is EnvironmentDumpActionManager.Update || _action is EnvironmentDumpActionManager.Reset)
        {
            ConsoleDraw.DrawFeedBack("DataBase: ");
            _scriptRunner.Run(_action is EnvironmentDumpActionManager.Reset);
        }

        _serviceCollection.AddLogging(config =>
        {
            config.AddDebug();
            config.AddConsole();
        });

        _serviceCollection.AddScoped<IExecutionContext, Infrastructure.Manager.Common.ExecutionContext>();
        _serviceCollection.AddApplication();
        _serviceCollection.AddInfracstruture(_configuration);

        _serviceCollection.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationConfiguration).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            }
        );

        AddDumps(_serviceCollection);

        _serviceProvider = _serviceCollection.BuildServiceProvider();

        ConsoleDraw.DrawFeedBack("Dumps: ");

        var dumps = GetClassDump().Select(a => _serviceProvider.GetService(a) as IDump).OrderBy(d => d.Order);

        foreach (var dump in dumps)
            await dump.DumpAsync();

        ConsoleDraw.DrawFeedBack(string.Empty);
    }

    private static Type[] GetClassDump()
    {
        return (from asm in AppDomain.CurrentDomain.GetAssemblies()
                from type in asm.GetTypes()
                where type.IsClass && type.Name.EndsWith("Dump")
                select type).ToArray();
    }

    private void AddDumps(IServiceCollection services)
    {
        // Not work
        // services.Scan(scan => scan
        //     .FromAssemblyOf<Program>()
        //     .AddClasses(classes => classes.AssignableTo(typeof(Dump<>))
        //         .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDump))))
        //     .AsImplementedInterfaces()
        //     .WithScopedLifetime()
        // );

        foreach (var item in GetClassDump())
            services.AddScoped(item);
    }
}
using Application.Common.Interfaces;
using Domain.Common.Interfaces;
using Domain.Core.Interfaces;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfracstruture(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbPersistence.Configure();

        services.AddDbContext<SqlContext>(
            options => options.UseSqlServer(
                configuration.GetConnectionString("ConnSql"), b => b.MigrationsAssembly(typeof(SqlContext).Assembly.FullName)
            )
        );

        services.AddStackExchangeRedisCache(options =>
        {
           options.Configuration = configuration.GetConnectionString("ConnCache");
           options.InstanceName = configuration.GetValue<string>("Environment:dentifier");
        });

        services.AddGlobalization();
        services.AddScoped<SqlContext>();
        services.AddScoped<NoSqlContext>();
        services.AddSingleton<ICache, CacheContext>();

        //Audit
        services.AddSingleton<IAuditRepository, AuditRepository>();

        services.Scan(scan => scan
            .FromAssemblyOf<SqlContext>()
            .AddClasses(classes => classes.AssignableTo(typeof(Repository<>))
                .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        return services;
    }
}
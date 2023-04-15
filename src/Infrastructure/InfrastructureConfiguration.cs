using Application.Common.Interfaces;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfracstruture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlContext>(
            options => options.UseSqlServer(
                configuration.GetConnectionString("ConnSql"), b => b.MigrationsAssembly(typeof(SqlContext).Assembly.FullName)
            )
        );

        services.AddScoped<SqlContext>();

        services.AddSingleton<ICache, CacheContext>();

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

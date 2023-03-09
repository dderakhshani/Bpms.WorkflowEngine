using System;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bpms.WorkflowEngine.Configuration
{
    public static class MsSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguredMsSqlDbContext<TUnitOfWork>(this IServiceCollection services, string connectionString) where TUnitOfWork : DbContext
        {
            services.AddDbContextPool<TUnitOfWork>((serviceProvider, optionsBuilder) =>
            optionsBuilder
                .UseSqlServer(
                    connectionString,
                    sqlServerOptionsBuilder =>
                    {
                        sqlServerOptionsBuilder
                            .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                            .EnableRetryOnFailure()
                            .MigrationsAssembly(typeof(MsSqlServiceCollectionExtensions).Assembly.FullName)
                            .UseNetTopologySuite();
                    })
                .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));

            return services;
        }
    }
}
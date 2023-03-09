using System;
using System.Collections.Generic;
using System.Reflection;
using EasyCaching.InMemory;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bpms.WorkflowEngine.Databases.SqlServer;
using Bpms.WorkflowEngine.Databases.SqlServer.Context;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Bpms.WorkflowEngine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using Bpms.WorkflowEngine.Services;

namespace Bpms.WorkflowEngine.Configuration
{
    public static class ConfigurationExtentions
    {
        public static void UseBpms(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                 scope.ServiceProvider.GetRequiredService<IWorkflowInit>();
            }
        }

        public static void IncludeBpmsServices<T>(this IServiceCollection services, string connectionString, string performersQuery, List<PerformerQueryConfig> perofrmerQueryConfigs) where T : DbContext, IBpmsUnitOfWork
        {
            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            }); ;
            services.AddMvc();
            services.AddMediatR(Assembly.GetAssembly(typeof(MediatRCore)));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IAdoContext, AdoContext>(x => new AdoContext(connectionString));
            services.AddScoped<IUnitOfWork, T>();
            services.AddScoped<IBpmsUnitOfWork, T>();
            services.AddDbContext<T>(options => options.UseSqlServer(connectionString),
                ServiceLifetime.Transient);
            //services.UseCaching<T>(new[]
            //{
            //    typeof(Performer),
            //    typeof(Workflow),
            //    typeof(Activity),
            //    typeof(BusinessRule),
            //    typeof(BusinessRuleCondition),
            //    typeof(TransitionShape),
            //    typeof(Shape),
            //    typeof(Event),
            //    typeof(Lane),
            //    typeof(Performer),
            //    typeof(PerformerCondition),
            //    typeof(Pool),
            //    typeof(Process),
            //    typeof(RuntimeActivity),
            //    typeof(RuntimeProcess),
            //    typeof(RuntimeWorkflow),
            //    typeof(RuntimeWorkflowParameter),
            //    typeof(ServiceActivity),
            //    typeof(Task),
            //    typeof(TaskComment),
            //    typeof(TaskWorkHistory),
            //    typeof(Transition),
            //    typeof(UiForm),
            //    typeof(UiFormControl),
            //    typeof(UiFormVariable),
            //    typeof(UiGrid),
            //    typeof(UiGridColumn),
            //    typeof(Workflow)
            //}, connectionString);
            services.AddScoped<IUser, User>();
            services.AddScoped<IWorkflowInit, WorkflowInit>();
            services.AddScoped<IWorkflowEngineEssentials, WorkflowEngineEssentials>(x => new WorkflowEngineEssentials(performersQuery));
            services.AddScoped<IWorkflowEngine, WorkflowEngine>();
            services.AddScoped<IWorkflowDefinition, WorkflowDefinition>();
            services.AddScoped<IBpmsAdminService, BpmsAdminService>();
            services.AddSingleton<IPerformerQueryConfigsSingletoon, PerformerQueryConfigsSingletoon>(x => new PerformerQueryConfigsSingletoon(perofrmerQueryConfigs));

            services.AddScoped<IValidator<BpmsAdminService.CreateWorkflowModel>, BpmsAdminService.CreateWorkflowModelValidator>();
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());



        }


        private static void UseCaching<TUnitOfWork>(this IServiceCollection services, Type[] types, string connectionString) where TUnitOfWork : DbContext
        {
            services.AddConfiguredMsSqlDbContext<TUnitOfWork>(connectionString);

            var provider = "bpms_inmemory";
            services.AddEFSecondLevelCache(options =>
                options.UseEasyCachingCoreProvider(provider, isHybridCache: false)
                    .DisableLogging(false)
                    .CacheQueriesContainingTypes(CacheExpirationMode.Sliding, TimeSpan.FromHours(8), types)
                    .UseCacheKeyPrefix("EF_")
            );
            services.AddEasyCaching(options =>
            {
                // use memory cache with your own configuration
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = (int)TimeSpan.FromHours(1).TotalSeconds,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100000,

                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = false,
                        // enable deep clone when writing object to cache or not, default value is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = (int)TimeSpan.FromHours(8).TotalSeconds;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, provider);
            });
        }
    }


    public interface IPerformerQueryConfigsSingletoon
    {
        List<PerformerQueryConfig> PerformerQueryConfigs { get; }
    }

    public class PerformerQueryConfigsSingletoon : IPerformerQueryConfigsSingletoon
    {
        public PerformerQueryConfigsSingletoon(List<PerformerQueryConfig> performerQueryConfigs)
        {
            PerformerQueryConfigs = performerQueryConfigs;
        }
        public List<PerformerQueryConfig> PerformerQueryConfigs { get; }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Infrastructure;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Activity = Bpms.WorkflowEngine.Databases.Entities.Activity;
using Process = Bpms.WorkflowEngine.Databases.Entities.Process;
using Task = Bpms.WorkflowEngine.Databases.Entities.Task;

namespace Bpms.WorkflowEngine.Databases.SqlServer.Context
{
    public partial class BpmsUnitOfWork : DbContext, IBpmsUnitOfWork
    {
        public virtual DbSet<Activity> Activities { get; set; } = default!;
        public virtual DbSet<BusinessRule> BusinessRules { get; set; } = default!;
        public virtual DbSet<BusinessRuleCondition> BusinessRuleConditions { get; set; } = default!;
        public virtual DbSet<TransitionShape> TransitionShapes { get; set; }
        public virtual DbSet<Shape> Shapes { get; set; }
        public virtual DbSet<Event> Events { get; set; } = default!;
        public virtual DbSet<Lane> Lanes { get; set; } = default!;
        public virtual DbSet<Performer> Performers { get; set; } = default!;
        public virtual DbSet<PerformerCondition> PerformerConditions { get; set; } = default!;
        public virtual DbSet<Pool> Pools { get; set; } = default!;
        public virtual DbSet<Process> Processes { get; set; } = default!;
        public virtual DbSet<RuntimeActivity> RuntimeActivities { get; set; } = default!;
        public virtual DbSet<RuntimeProcess> RuntimeProcesses { get; set; } = default!;
        public virtual DbSet<RuntimeWorkflow> RuntimeWorkflows { get; set; } = default!;
        public virtual DbSet<RuntimeWorkflowParameter> RuntimeWorkflowParameters { get; set; } = default!;
        public virtual DbSet<ServiceActivity> ServiceActivities { get; set; } = default!;
        public virtual DbSet<Task> Tasks { get; set; } = default!;
        public virtual DbSet<TaskComment> TaskComments { get; set; } = default!;
        public virtual DbSet<TaskWorkHistory> TaskWorkHistories { get; set; } = default!;
        public virtual DbSet<Transition> Transitions { get; set; } = default!;
        public virtual DbSet<UiForm> UiForms { get; set; } = default!;
        public virtual DbSet<UiFormControl> UiFormControls { get; set; } = default!;
        public virtual DbSet<UiFormVariable> UiFormVariables { get; set; } = default!;
        public virtual DbSet<UiGrid> UiGrids { get; set; } = default!;
        public virtual DbSet<UiGridColumn> UiGridColumns { get; set; } = default!;
        public virtual DbSet<Workflow> Workflows { get; set; } = default!;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public DbContext DbContext()
        {
            return this;
            
        }


        public BpmsUnitOfWork(DbContextOptions<BpmsUnitOfWork> options)
            : base(options)
        {
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            //optionsBuilder.UseSqlServer("Data Source=192.168.14.13\\sql2019;Initial Catalog=EefaDev;User Id=ssrs;Password=123456").LogTo(x => Debug.WriteLine(x));
            //  optionsBuilder.UseSqlServer(ConnectionString).LogTo(x => Debug.WriteLine(x));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class EfFilterExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EfFilterExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod);

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }

}
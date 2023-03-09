using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.SqlServer.Context
{
    public interface IBpmsUnitOfWork : IUnitOfWork
    {
        DbSet<Activity> Activities { get; set; } 
        DbSet<BusinessRule> BusinessRules { get; set; }  
        DbSet<BusinessRuleCondition> BusinessRuleConditions { get; set; }  
        DbSet<TransitionShape> TransitionShapes { get; set; }  
        DbSet<Shape> Shapes { get; set; }  
        DbSet<Event> Events { get; set; }  
        DbSet<Lane> Lanes { get; set; }  
        DbSet<Performer> Performers { get; set; }  
        DbSet<PerformerCondition> PerformerConditions { get; set; }  
        DbSet<Pool> Pools { get; set; }  
        DbSet<Process> Processes { get; set; }  
        DbSet<RuntimeActivity> RuntimeActivities { get; set; }  
        DbSet<RuntimeProcess> RuntimeProcesses { get; set; }  
        DbSet<RuntimeWorkflow> RuntimeWorkflows { get; set; }  
        DbSet<RuntimeWorkflowParameter> RuntimeWorkflowParameters { get; set; }  
        DbSet<ServiceActivity> ServiceActivities { get; set; }  
        DbSet<Task> Tasks { get; set; }  
        DbSet<TaskComment> TaskComments { get; set; }  
        DbSet<TaskWorkHistory> TaskWorkHistories { get; set; }  
        DbSet<Transition> Transitions { get; set; }  
        DbSet<UiForm> UiForms { get; set; }  
        DbSet<UiFormControl> UiFormControls { get; set; }  
        DbSet<UiFormVariable> UiFormVariables { get; set; }  
        DbSet<UiGrid> UiGrids { get; set; }  
        DbSet<UiGridColumn> UiGridColumns { get; set; }  
        DbSet<Workflow> Workflows { get; set; }

        DbContext DbContext();
    }
}
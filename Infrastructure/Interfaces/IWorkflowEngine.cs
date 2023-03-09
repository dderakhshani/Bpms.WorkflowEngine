using System.Collections.Generic;
using System.Threading.Tasks;
using Bpms.WorkflowEngine.Databases.Entities;

namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface IWorkflowEngine
    {
        Task<RuntimeActivity> StartWorkflow(string uniqueName, int userId);
        Task<RuntimeWorkflow> GetRuntimeWfByRuntimeActivityId(int runningActivityId);
        System.Threading.Tasks.Task ActivateNextActivities(RuntimeActivity runningActivity, int priority, int primaryKeyValue, string taskTitle = "", Dictionary<string, string> paramValues = null);
        System.Threading.Tasks.Task ActivateNextActivities(int runningActivityId, int priority, int primaryKeyValue, string taskTitle = "");
        System.Threading.Tasks.Task SetRuntimeActivityStatus(int taskId, short status);
        System.Threading.Tasks.Task CallAttachedEvent(int taskId, int primaryKeyValue, string eventName);
        System.Threading.Tasks.Task CreateTasks(RuntimeProcess runningProcess, Activity activity, string taskTitle, int priority);
    }
}
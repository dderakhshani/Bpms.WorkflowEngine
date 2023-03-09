using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Models;

namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface ITaskInboxManager
    {
        Task<int> GetUserId(string employeeCode);
        Task<bool> IsLogined(bool isLogined, int userId);
        Task<List<RuntimeWorkflowParameter>> GetParametersOfTask(int taskId);
        Task<IQueryable<TaskModel>> GetTasks(int userId, string orderFiled, bool isDescending, bool searchAll, string fTime, string tTime);
        Task<List<TaskModel>> GetTasks(int userId);
        Task<TaskModel> StartTask(int taskId, int userId);
        Task<TaskModel> PauseTasks(int taskId);
        Task<TaskModel> ReadTasks(int taskId);
        Task<TaskModel> RejectTasks(int taskId);
        Task<TaskModel> ContinueTasks(int taskId, int userId);
        Task<List<TaskModel>> GetHistoryTasks(int taskId);

        Task<bool> EndTask(int taskId, int currentUserId, int primaryKeyValue,
            Dictionary<string, string> paramValues = null, bool activateNextActivity = true);
    }
}
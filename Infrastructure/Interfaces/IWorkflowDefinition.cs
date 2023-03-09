using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Models;

namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface IWorkflowDefinition
    {
        Task<Activity> GetActivity(int activityId);
        Task<RuntimeActivity> GetActivityByTaskId(int taskId);
        Task<List<Workflow>> GetWorkflows();

         Task<Workflow> ImportXpdl(string path,
             Workflow workflow);
        Task<bool> AddPerformers(List<PossiblePerformerModel> performerList);
        Task<bool> UpdateActivity(Activity activity);
        Task<int> GetActivityState(int taskId, int activityId);
        Task<List<GraphicShape>> GetShapesForGraphicalWorkflow(int workflowId, int taskId);
        Task<List<Transition>> GetTransitions(int workflowId);
        Task<List<Shape>> GetTransitionCoordinates(int workflowId);
        Task<List<PossiblePerformerModel>> GetPossiblePerformers();
        Task<List<int>> GetTransitionIds(int workflowId);
        Task<List<int?>> GetCoordinatesArray(int transitionId);
        Task<List<TransitionLine>> GetTransitionLines(int workflowId);
    }
}
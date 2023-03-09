using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Bpms.WorkflowEngine.Configuration;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Databases.SqlServer.Context;
using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Task = Bpms.WorkflowEngine.Databases.Entities.Task;

namespace Bpms.WorkflowEngine
{
    // Workflow excution process goes here(such as run the workflow, add,get tasks and ....)


    public class WorkflowEngine : IWorkflowEngine
    {
        private readonly IWorkflowEngineEssentials _workflowEngineEssentials;
        private readonly IBpmsUnitOfWork _bpmsUnitOfWork;
        private readonly IAdoContext _adoContext;
        private readonly IUser _currentUser;
        private readonly IUser _creatorUser;
        private readonly IPerformerQueryConfigsSingletoon _performerQueryConfigsSingletoon;
        public WorkflowEngine(IWorkflowEngineEssentials workflowEngineEssentials, IUser currentUser, IUser creatorUser, IBpmsUnitOfWork bpmsUnitOfWork, IAdoContext adoContext, IPerformerQueryConfigsSingletoon performerQueryConfigsSingletoon)
        {
            _workflowEngineEssentials = workflowEngineEssentials;
            _currentUser = currentUser;
            _currentUser.UserId = 1;
            _creatorUser = creatorUser;
            _creatorUser.UserId = 1;
            _bpmsUnitOfWork = bpmsUnitOfWork;
            _adoContext = adoContext;
            _performerQueryConfigsSingletoon = performerQueryConfigsSingletoon;
        }

        #region ---------------------Public Methods---------------------
        public async Task<RuntimeActivity> StartWorkflow(string uniqueName, int userId)
        {
            var workflow = await _bpmsUnitOfWork.Workflows.FirstAsync(c => c.UniqueName == uniqueName);

            var insertedRuntimeWorkflow = _bpmsUnitOfWork.RuntimeWorkflows.Add(new RuntimeWorkflow()
            {
                WorkflowId = workflow.Id,
                StartDate = DateTime.Now,
                CreatorId = userId
            });

            var process =
                await _bpmsUnitOfWork.Processes.FirstAsync(c =>
                    c.WorkflowId == workflow.Id && (bool)!c.IsActivitySet);

            var insertedRuntimeProcess = _bpmsUnitOfWork.RuntimeProcesses.Add(new RuntimeProcess()
            {
                RuntimeWorkflow = insertedRuntimeWorkflow.Entity,
                ProcessId = process.Id,
                StartDate = DateTime.Now
            });

            var startEvent = await FindStartEvent(process.Id);
            var firstTransition =
                await _bpmsUnitOfWork.Transitions.FirstOrDefaultAsync(c => c.From == startEvent.Activity.Guid);
            var firstActivity =
                await _bpmsUnitOfWork.Activities.FirstOrDefaultAsync(c => c.Guid == firstTransition.To);

            var insertedRuningActivity = _bpmsUnitOfWork.RuntimeActivities.Add(new RuntimeActivity()
            {
                Activity = firstActivity,
                RuntimeProcess = insertedRuntimeProcess.Entity,
                StartDate = DateTime.Now,
                CreatorId = userId
            });

            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

            return insertedRuningActivity.Entity;
        }

        public async Task<RuntimeWorkflow> GetRuntimeWfByRuntimeActivityId(int runningActivityId)
        {
            var runningActivity = await _bpmsUnitOfWork.RuntimeActivities
                .FirstOrDefaultAsync(c => c.Id == runningActivityId);

            runningActivity.IsFinished = true;
            runningActivity.ProgressStatus = 100;
            runningActivity.EndDate = DateTime.Now;

            var runningProcess = await _bpmsUnitOfWork.RuntimeProcesses
                .FirstOrDefaultAsync(c => c.Id == runningActivity.RuntimeProcessId);

            return await _bpmsUnitOfWork.RuntimeWorkflows
                .FirstOrDefaultAsync(c => c.Id == runningProcess.RuntimeWorkflowId);
        }

        public async System.Threading.Tasks.Task ActivateNextActivities(RuntimeActivity runningActivity,
            int priority, int primaryKeyValue, string taskTitle = "", Dictionary<string, string> paramValues = null)
        {
            runningActivity.IsFinished = true;
            runningActivity.EndDate = DateTime.Now;
            var currentActivity =
                await _bpmsUnitOfWork.Activities.FirstOrDefaultAsync(c => c.Id == runningActivity.ActivityId);

            var transitionsToGuid = await _bpmsUnitOfWork.Transitions
                .Where(c => c.From == currentActivity.Guid)
                .Select(x => x.To).ToListAsync();

            var nextActivities = await _bpmsUnitOfWork.Activities
                .Where(a => transitionsToGuid.Contains(a.Guid))
                .ToListAsync(); 

            foreach (var nextActivity in nextActivities)
            {
                await RunActivity(nextActivity, runningActivity, primaryKeyValue, taskTitle, priority, paramValues);
            }
        }

        public async System.Threading.Tasks.Task ActivateNextActivities(int runningActivityId, int priority,
            int primaryKeyValue, string taskTitle = "")
        {
            var runningActivity = await _bpmsUnitOfWork.RuntimeActivities
                .Include(x => x.RuntimeProcess)
                .FirstAsync(x => x.Id == runningActivityId);

            await ActivateNextActivities(runningActivity, priority, primaryKeyValue, taskTitle);
        }

        public async System.Threading.Tasks.Task SetRuntimeActivityStatus(int taskId, short status)
        {
            var raEntity = await (from t in _bpmsUnitOfWork.Tasks
                                  join ra in _bpmsUnitOfWork.RuntimeActivities on t.RuntimeActivityId equals ra.Id
                                  where t.Id == taskId
                                  select ra).FirstAsync();
            raEntity.Status = status;

            _bpmsUnitOfWork.RuntimeActivities.Update(raEntity);
            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
        }

        public async System.Threading.Tasks.Task CallAttachedEvent(int taskId, int primaryKeyValue,
            string eventName)
        {
            var targetActivity = await (from t in _bpmsUnitOfWork.Tasks
                                        join ra in _bpmsUnitOfWork.RuntimeActivities.Include(x => x.RuntimeProcess)
                                            on t.RuntimeActivityId
                                            equals ra.Id
                                        join rp in _bpmsUnitOfWork.RuntimeProcesses on ra.RuntimeProcessId equals rp.Id
                                        join a in _bpmsUnitOfWork.Activities on ra.ActivityId equals a.Id
                                        where t.Id == taskId
                                        select new { a, ra, rp, t }).FirstAsync();

            var attachedEventActivity = await (from a in _bpmsUnitOfWork.Activities
                                               join e in _bpmsUnitOfWork.Events on a.Id equals e.Id
                                               where e.Target == targetActivity.a.Guid
                                               select a).FirstAsync();

            var ra1 = new RuntimeActivity()
            {
                Activity = attachedEventActivity,
                CreatorId = _currentUser.UserId,
                IsFinished = true,
                ProgressStatus = 100,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                RuntimeProcess = targetActivity.rp
            };

            _bpmsUnitOfWork.RuntimeActivities.Add(ra1);
            await ActivateNextActivities(ra1, targetActivity.t.Priority, primaryKeyValue, targetActivity.t.Title);
        }

        public async System.Threading.Tasks.Task CreateTasks(RuntimeProcess runningProcess, Activity activity,
            string taskTitle, int priority)
        {
            //for finding creator user in GetPerformersForActivity
            var runningWorkflow =
                await _bpmsUnitOfWork.RuntimeWorkflows.FirstAsync(x => x.Id == runningProcess.RuntimeWorkflowId);

            var performerList = await GetActivityPerformers(activity.Id, runningWorkflow);

            var insertedRunnigActivity = _bpmsUnitOfWork.RuntimeActivities.Add(new RuntimeActivity()
            {
                ActivityId = activity.Id,
                RuntimeProcessId = runningProcess.Id,
                StartDate = DateTime.Now,
                Status = activity.DefaultStatus
            });

            foreach (var task in performerList.Select(userId => new Task()
            {
                UserId = userId,
                RuntimeActivity = insertedRunnigActivity.Entity,
                CreateTime = DateTime.Now,
                StartTime = DateTime.Now,
                RuntimeWorkflowId = runningProcess.RuntimeWorkflowId,
                CreatorUserId = _currentUser.UserId,
                Status = 1,
                Priority = priority,
                ProgressStatus = 0,
                Title = taskTitle
            }))
            {
                _bpmsUnitOfWork.Tasks.Add(task);
            }

            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
        }

        #endregion

        #region ---------------------Private Methods--------------------

        public async Task<string> EvaluatePerformerCondition(PerformerCondition performerCondition)
        {
            var whereResult = " ";
            if (performerCondition.NodeType == PerformerConditionNodeType.Condition)
            {
                var value = performerCondition.StaticValue;
                
                var fieldName = _performerQueryConfigsSingletoon.PerformerQueryConfigs
                    .First(x => x.PerformerType == performerCondition.PerformerType)
                    .FieldName;

                if (performerCondition.ValueSourceType != ValueSourceType.Static)
                {
                    var user = performerCondition.ValueSourceType switch
                    {
                        ValueSourceType.Creator => _creatorUser,
                        ValueSourceType.Current => _currentUser,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    value = performerCondition.PerformerType switch
                    {
                        PerformerTypes.User => user.UserId.ToString(),
                        PerformerTypes.Position => user.PositionId.ToString(),
                        PerformerTypes.Unit => user.UnitId.ToString(),
                        PerformerTypes.ParentUnit => user.ParentUnitId.ToString(),
                        PerformerTypes.Role => user.RoleId.ToString(),
                        PerformerTypes.ParentRole => user.ParentRoleId.ToString(),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }

                var op = "";
                if (performerCondition.IsEqual != null)
                    op = ((bool)performerCondition.IsEqual) ? " = " : " != ";
                return fieldName + op + value;
            }

            whereResult += " ( ";
            var childs = await _bpmsUnitOfWork.PerformerConditions
                .Where(c => c.ParentId == performerCondition.Id).ToListAsync();
            var i = 0;
            foreach (var child in childs)
            {
                var type = performerCondition.NodeType switch
                {
                    PerformerConditionNodeType.Condition => "Condition",
                    PerformerConditionNodeType.And => "And",
                    PerformerConditionNodeType.Or => "Or",
                    _ => throw new ArgumentOutOfRangeException()
                };

                whereResult += EvaluatePerformerCondition(child);
                i++;
                if (i < childs.Count)
                    whereResult += type;
            }

            whereResult += " ) ";
            return whereResult;
        }

        private async Task<List<int>> GetActivityPerformers(int activityId, RuntimeWorkflow runningWorkflow)
        {
            var userIds = new List<int>();
            var performers = await _bpmsUnitOfWork.Performers.Where(c => c.ActivityId == activityId).ToListAsync();
            var allUsers = new List<int>();
            var currentUser = _currentUser;
            var creatorUser = _creatorUser;

            foreach (var performer in performers)
            {
                if (performer != null)
                {
                    var query = _workflowEngineEssentials.PerformersQuery;

                    var root = await _bpmsUnitOfWork.PerformerConditions
                        .FirstAsync(c => c.PerformerId == performer.Id && c.ParentId == null);
                    
                    query += await EvaluatePerformerCondition(root);

                    userIds = await _adoContext.GetAll<int>(query);
                }


                if (performer != null)
                    switch (performer.AssignationMethod)
                    {
                        case AssignationMethods.All:
                            allUsers.AddRange(userIds);
                            break;
                        case AssignationMethods.ByLoad:
                            return userIds;
                        case AssignationMethods.Sequencial:
                            return userIds;
                        case AssignationMethods.AvailableUsers:
                            var availableUsers = userIds;
                            if (availableUsers.Count > 0)
                                return availableUsers;
                            break;
                    }
            }

            return allUsers;
        }


        private async Task<Event> FindStartEvent(int processId)
        {
            var activityList = await _bpmsUnitOfWork.Activities
                .Where(c => c.ProcessId == processId)
                .ToListAsync();

            foreach (var activityItemId in activityList.Select(activityItem => activityItem.Id))
            {
                if (await _bpmsUnitOfWork.Events
                    .FirstOrDefaultAsync(c => c.Id == activityItemId && c.Type == 1) != null)
                    return await _bpmsUnitOfWork.Events
                        .FirstOrDefaultAsync(c => c.Id == activityItemId && c.Type == 1);
            }

            return null;
        }

        private async System.Threading.Tasks.Task RunActivity(Activity activity, RuntimeActivity senderRActivity,
            int primaryKeyValue, string taskTitle, int priority, Dictionary<string, string> paramValues = null)
        {
            if (activity.Type == (int)ActivityTypes.EndEvent)
            {
                senderRActivity.IsFinished = true;
                senderRActivity.EndDate = DateTime.Now;
                var tokens = await (from ra in _bpmsUnitOfWork.RuntimeActivities.Include("Activity")
                                    where ra.Id != senderRActivity.Id
                                          && ra.RuntimeProcessId == senderRActivity.RuntimeProcessId
                                          && (ra.IsFinished == null || ra.IsFinished == false)
                                    select ra).ToListAsync();

                if (tokens.Count == 0)
                {
                    senderRActivity.RuntimeProcess.EndDate = DateTime.Now;
                }
            }
            else if (activity.Type == (int)ActivityTypes.Activity)
                await CreateTasks(senderRActivity.RuntimeProcess, activity, taskTitle, priority);
            else if (activity.Type == (int)ActivityTypes.ServiceActivity)
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await InvokeServiceAsync(activity, paramValues, senderRActivity.RuntimeProcessId,
                        primaryKeyValue, taskTitle, priority);
                }).Wait();
            else if (activity.Type is (int)ActivityTypes.NormalGateway or
                (int)ActivityTypes.InclusiveGateWay or
                (int)ActivityTypes.ExclusiveGateWay or
                (int)ActivityTypes.ParallelGateWay)
            {
                if (activity.Type == (int)ActivityTypes.InclusiveGateWay)
                {
                    //Is there any active token(Runned activity in workflow)
                    var tokens = (from ra in _bpmsUnitOfWork.RuntimeActivities.Include("Activity")
                                  where ra.Id != senderRActivity.Id
                                        && ra.RuntimeProcessId == senderRActivity.RuntimeProcessId
                                        && (ra.IsFinished == null || ra.IsFinished == false)
                                  select ra).ToList();

                    if (tokens.Count > 0)
                    {
                        foreach (var ra in tokens)
                            if (await HasRoute(activity, activity))
                                return;
                    }
                }

                var transitionListFromGateway = await _bpmsUnitOfWork.Transitions
                    .Where(c => c.From == activity.Guid)
                    .ToListAsync();

                if (transitionListFromGateway.Count > 1)
                {
                    foreach (var tr in transitionListFromGateway)
                    {
                        if (!await EvaluatePassBusinessRule(tr, primaryKeyValue)) continue;
                        var nextActivity = await _bpmsUnitOfWork.Activities.SingleAsync(c => c.Guid == tr.To);
                        await RunActivity(nextActivity, senderRActivity, primaryKeyValue, taskTitle, priority,
                            paramValues);
                    }
                }
                else
                {
                    var guid = transitionListFromGateway[0].To;
                    var nextActivity = await _bpmsUnitOfWork.Activities.FirstAsync(c => c.Guid == guid);
                    await RunActivity(nextActivity, senderRActivity, primaryKeyValue, taskTitle, priority,
                        paramValues);
                }
            }
        }

        private async Task<bool> EvaluatePassBusinessRule(Transition tr, int primaryKeyValue)
        {
            var br = await _bpmsUnitOfWork.BusinessRules
                .FirstOrDefaultAsync(x => x.Id == tr.BusinessRuleId);

            if (br == null)
                return true;

            var pass = true;
            var query = "";
            var noTimeUsage = true;

            var timePass = br.CombinationType != CombinationTypes.Or;
            {
                var first = true;
                var brConditions = await _bpmsUnitOfWork.BusinessRuleConditions
                    .Where(x => x.BusinessRuleId == br.Id)
                    .ToListAsync();
                foreach (var brCondition in brConditions)
                {
                    if (brCondition.TableName == "Time")
                    {
                        noTimeUsage = false;
                        var nowTime00 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0,
                            0);
                        if (brCondition.FieldName == "Now")
                        {
                            var now = DateTime.Now;
                            var valueTimes = brCondition.Value.Split(':');

                            nowTime00 = nowTime00.AddHours(double.Parse(valueTimes[0]));
                            nowTime00 = nowTime00.AddMinutes(double.Parse(valueTimes[1]));

                            var evalResult = brCondition.Operator switch
                            {
                                Comparison.Equal => nowTime00 == now,
                                Comparison.LessThan => now < nowTime00,
                                Comparison.GreaterThan => now > nowTime00,
                                Comparison.LessThanOrEqual => now <= nowTime00,
                                Comparison.GreateThanOrEqual => now >= nowTime00,
                                Comparison.In => nowTime00 == now,
                                Comparison.NotIn => nowTime00 != now,
                                _ => throw new ArgumentOutOfRangeException()
                            };


                            if (br.CombinationType == CombinationTypes.And)
                                timePass = timePass && evalResult;
                            else
                                timePass = timePass || evalResult;
                        }

                        continue;
                    }


                    if (first)
                    {
                        query += brCondition.TableName + " WHERE ";
                        first = false;
                    }

                    if (!query.EndsWith(" WHERE "))
                        query += br.CombinationType == CombinationTypes.And ? " AND " : " OR ";

                    var op = brCondition.Operator switch
                    {
                        Comparison.Equal => "=",
                        Comparison.LessThan => "<",
                        Comparison.GreaterThan => ">",
                        Comparison.LessThanOrEqual => "<=",
                        Comparison.GreateThanOrEqual => ">=",
                        Comparison.In => " IN ",
                        Comparison.NotIn => " NOT IN ",
                        Comparison.Like => " Like ",
                        Comparison.NotEqualTo => " <> ",
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    query += brCondition.FieldName + op + $"N'{brCondition.Value}'";
                    if (brCondition.ForeignKeyName != null)
                        query += " And " + brCondition.ForeignKeyName + " = " + primaryKeyValue;
                }

                if (query != "")
                {
                    query = "SELECT * FROM " + query;
                    var res = await _adoContext.GetAll(query);
                        pass = res is { Count: > 0 };
                }

                //if no time usage ignore timePass
                pass = pass && (timePass || noTimeUsage);
            }

            return (pass && br.Invert == false) || (!pass && br.Invert == true);
        }


        private async System.Threading.Tasks.Task InvokeServiceAsync(Activity activity,
            IReadOnlyDictionary<string, string> paramValues, int runningProcessId, int primaryKeyValue,
            string taskTitle, int priority)
        {
            var serviceActivity = await _bpmsUnitOfWork.ServiceActivities.FirstAsync(x => x.Id == activity.Id);
            var postData = CompileParameter(serviceActivity.Parameters, paramValues);

            var request = WebRequest.Create(serviceActivity.Url);
            request.Method = "POST";

            var byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            try
            {
                var dataStream = await request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = null;
                response = await request.GetResponseAsync();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);
                    var responseFromServer = await reader.ReadToEndAsync();
                    Console.WriteLine(responseFromServer);
                    reader.Close();
                }

                dataStream?.Close();

                response.Close();
                var ra1 = new RuntimeActivity()
                {
                    Activity = activity,
                    CreatorId = _currentUser.UserId,
                    IsFinished = true,
                    ProgressStatus = 100,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    RuntimeProcessId = runningProcessId

                };
                _bpmsUnitOfWork.RuntimeActivities.Add(ra1);
                await ActivateNextActivities(ra1, priority, primaryKeyValue, taskTitle);
            }
            catch (WebException)
            {

            }
        }

        private string CompileParameter(string parameters, IReadOnlyDictionary<string, string> values)
        {
            var startIndex = -1;
            var index = 0;
            var compiledparam = "";
            foreach (var c in parameters)
            {
                switch (c)
                {
                    case '{':
                        startIndex = index;
                        break;
                    case '}':
                        {
                            var param = parameters.Substring(startIndex + 1, index - startIndex - 1);
                            compiledparam += values[param];
                            startIndex = -1;
                            break;
                        }
                    default:
                        {
                            if (startIndex == -1)
                                compiledparam += c;
                            break;
                        }
                }

                index++;
            }

            return compiledparam;
        }

        /// <summary>
        /// Is There any route path between srcActivity and destActivity
        /// </summary>
        /// <param name="srcActivity"></param>
        /// <param name="destActivity"></param>
        /// <param name="visitedActivities">Helper paramters to use in recursive call, Pass Null as a default</param>
        /// <returns></returns>
        private async Task<bool> HasRoute(Activity srcActivity, Activity destActivity,
            List<int> visitedActivities = null)
        {
            var toTrans = await _bpmsUnitOfWork.Transitions
                .Where(x => x.From == srcActivity.Guid)
                .Select(x => x.To)
                .ToListAsync();

            var descSctivities = await (from a in _bpmsUnitOfWork.Activities
                                        where toTrans.Contains(a.Guid)
                                        select a).ToListAsync();

            visitedActivities ??= new List<int> { srcActivity.Id };

            foreach (var a in descSctivities)
                if (a == destActivity)
                    return true;
                else if (a.Type == (int)ActivityTypes.EndEvent)
                {
                    visitedActivities.Add(a.Id);
                    continue;
                }
                else if (visitedActivities.Contains(a.Id))
                {
                    visitedActivities.Add(a.Id);
                    continue;
                }
                else
                {
                    var found = await HasRoute(a, destActivity, visitedActivities);
                    if (found)
                        return true;
                }

            return false;
        }

        #endregion


    }
}

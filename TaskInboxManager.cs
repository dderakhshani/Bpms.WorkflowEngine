//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using NetTopologySuite.GeometriesGraph;
//using Bpms.WorkflowEngine.Databases.Entities;
//using Bpms.WorkflowEngine.Databases.SqlServer.Context;
//using Bpms.WorkflowEngine.Infrastructure;
//using Bpms.WorkflowEngine.Infrastructure.Interfaces;
//using Bpms.WorkflowEngine.Models;
//using Task = Bpms.WorkflowEngine.Databases.Entities.Task;

//namespace Bpms.WorkflowEngine
//{
//    public class TaskInboxManager : ITaskInboxManager
//    {
//        private readonly IBpmsUnitOfWork _bpmsUnitOfWork;

//        public TaskInboxManager(IBpmsUnitOfWork bpmsUnitOfWork)
//        {
//            _bpmsUnitOfWork = bpmsUnitOfWork;
//        }


//        public Task<int> GetUserId(string employeeCode)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> IsLogined(bool isLogined, int userId)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<List<RuntimeWorkflowParameter>> GetParametersOfTask(int taskId)
//        {
//            var runningWorkflowIdId = Convert.ToInt32(await _bpmsUnitOfWork.Tasks
//                .Where(c => c.Id == taskId)
//                .Select(c => c.RuntimeWorkflowId).FirstAsync());

//            var q = await _bpmsUnitOfWork.RuntimeWorkflowParameters
//                .Where(c => c.RuntimeWorkflowId == runningWorkflowIdId)
//                .ToListAsync();


//            var relatedParameter = q.Select(item => new RuntimeWorkflowParameter
//            {
//                Id = item.Id,
//                ParameterName = item.ParameterName,
//                ParameterValue = item.ParameterValue,
//                ParameterDataType = item.ParameterDataType
//            }).ToList();
//            return relatedParameter.ToList();
//        }

//        public async Task<IQueryable<TaskModel>> GetTasks(int userId, string orderFiled, bool isDescending, bool searchAll, string fTime, string tTime)
//        {

//            DateTime? fromTime = null;
//            DateTime? toTime = null;
//            //  if (searchAll)
//            //  {
//            if (fTime != "NaN/NaN/NaN")
//                fromTime = fTime.ToDateTime();
//            if (tTime != "NaN/NaN/NaN")
//            {
//                toTime = tTime.ToDateTime();
//                toTime = ((DateTime)toTime).AddDays(1);//تا ابتدای روز بعد
//            }
//            //  }
//            var taskList = await (from t in _bpmsUnitOfWork.Tasks //.Where(c => c.Status != (int)TaskStatus.Finished)
//                                  join ra in _bpmsUnitOfWork.RuntimeActivities on t.RuntimeActivityId equals ra.Id
//                                  join rw in _bpmsUnitOfWork.RuntimeWorkflows on t.RuntimeWorkflowId equals rw.Id
//                                  join a in _bpmsUnitOfWork.Activities on ra.ActivityId equals a.Id
//                                  join u in _repository.GetQuery<User>() on t.CreatorUserId equals u.Id
//                                  join p in _repository.GetQuery<Person>() on u.PersonId equals p.Id
//                                  join e in _repository.GetQuery<Employee>() on p.Id equals e.PersonId

//                                  orderby t.Id descending
//                                  where t.UserId == userId
//                                  &&
//                                  ((!searchAll && t.Status < (int)TaskStatus.Finished && (fromTime < t.CreateTime && t.CreateTime < toTime))
//                                  ||
//                                  (searchAll && (fromTime < t.CreateTime && t.CreateTime < toTime)))
//                                  //orderby t.Id descending
//                                  select new TaskModel
//                                  {
//                                      ActivityName = a.Name,
//                                      CreateTime = t.CreateTime,
//                                      CreatorUserId = t.CreatorUserId,
//                                      CreatorName = p.LastName + " " + p.FirstName,
//                                      Description = t.Description,
//                                      StartTime = t.StartTime,
//                                      EndTime = t.EndTime,
//                                      ReadTime = t.ReadTime,
//                                      EmployeeCode = e.EmployeeCode,
//                                      RelatedTaskId = t.RelatedTaskId,
//                                      ProcessId = (int)a.ProcessId,
//                                      UserId = u.Id,
//                                      FormUrl = a.FormUrl,
//                                      PersonId = p.Id,
//                                      Id = t.Id,
//                                      PersonCode = e.EmployeeCode,
//                                      Title = t.Title,
//                                      ProgressStatus = t.ProgressStatus,
//                                      RuntimeWorkflowId = t.RuntimeWorkflowId,
//                                      DateDue = t.DateDue,
//                                      WorkflowId = (int)rw.WorkflowId,
//                                      Status = t.Status,
//                                      ActivityStatus = ra.Status,
//                                      Priority = t.Priority
//                                  }).ToListAsync();


//            foreach (var item in taskList)
//            {
//                item.CreateTimePersian = (((DateTime)item.CreateTime).ToPersianDate(DateOutPut.YMD));
//                if (item.StartTime != null && item.DateDue != null)
//                    item.DueStatus = (decimal)(((DateTime.Now - (DateTime)item.StartTime).TotalHours) / (((DateTime)item.DateDue - (DateTime)item.StartTime).TotalHours));
//                if (item.EndTime == null)
//                {
//                    item.FormUrl ??= "TaskInbox.EmptyTaskView";
//                }
//                else
//                    //item.Url= "TaskInbox.EmptyTaskView";
//                    item.FormUrl = "TaskInbox.WorkflowReportView";
//            }
//            var result = taskList.AsQueryable();

//            switch (orderFiled)
//            {
//                //if(orderFiled == "TaskId")
//                //{
//                //    result = result.OrderBy(c => c.Id);
//                //    if (isDescending)
//                //        result = result.OrderByDescending(c => c.Id);
//                //}
//                case "CreateTime":
//                    {
//                        result = result.OrderBy(c => c.CreateTime);
//                        if (isDescending)
//                            result = result.OrderByDescending(c => c.CreateTime);
//                        break;
//                    }
//                case "CreatorName":
//                    {
//                        result = result.OrderBy(c => c.CreatorName);
//                        if (isDescending)
//                            result = result.OrderByDescending(c => c.CreatorName);
//                        break;
//                    }
//                case "Type":
//                    {
//                        result = result.OrderBy(c => c.RuntimeWorkflowId);
//                        if (isDescending)
//                            result = result.OrderByDescending(c => c.RuntimeWorkflowId);
//                        break;
//                    }
//                case "DueStatus":
//                    {
//                        result = result.OrderBy(c => c.DueStatus);
//                        if (isDescending)
//                            result = result.OrderByDescending(c => c.DueStatus);
//                        break;
//                    }
//            }

//            return result;
//        }

//        public async Task<List<TaskModel>> GetTasks(int userId)
//        {
//            var query = await (from t in _bpmsUnitOfWork.Tasks
//                               join ra in _bpmsUnitOfWork.RuntimeActivities on t.RuntimeActivityId equals ra.Id
//                               join a in _bpmsUnitOfWork.Activities on ra.ActivityId equals a.Id

//                               join u in _repository.GetQuery<User>().Where(c => c.Id == userId) on t.UserId equals u.Id
//                               join u2 in _repository.GetQuery<User>() on t.CreatorUserId equals u2.Id

//                               join p in _repository.GetQuery<Person>() on u.PersonId equals p.Id
//                               join p2 in _repository.GetQuery<Person>() on u2.PersonId equals p2.Id

//                               join e in _repository.GetQuery<Employee>() on p.Id equals e.PersonId
//                               join e2 in _repository.GetQuery<Employee>() on p2.Id equals e2.PersonId

//                               join up in _repository.GetQuery<UnitPosition>() on e.UnitPositionId equals up.Id
//                               join pos in _repository.GetQuery<Position>() on up.PositionId equals pos.Id
//                               join unit in _repository.GetQuery<Unit>() on up.UnitId equals unit.Id
//                               where (t.UserId == userId) && (t.Status < 6)
//                               select new
//                               {
//                                   a,
//                                   t,
//                                   e,
//                                   p,
//                                   ra,
//                                   unit,
//                                   pos,
//                                   ProcessId = ra.RuntimeProcessId,
//                                   CreatorName = p2.FirstName + " " + p2.LastName
//                               }).ToListAsync();

//            var tasks = new List<TaskModel>();
//            foreach (var q in query)
//            {
//                if (q.t.StartTime != null)
//                    q.t.StartTime = new DateTime(q.t.StartTime.Value.Year, q.t.StartTime.Value.Month, q.t.StartTime.Value.Day, q.t.StartTime.Value.Hour, q.t.StartTime.Value.Minute, q.t.StartTime.Value.Second);
//                q.t.CreateTime = new DateTime(q.t.CreateTime.Year, q.t.CreateTime.Month, q.t.CreateTime.Day, q.t.CreateTime.Hour, q.t.CreateTime.Minute, q.t.CreateTime.Second);
//                if (q.t.ReadTime != null)

//                    q.t.ReadTime = new DateTime(q.t.ReadTime.Value.Year, q.t.ReadTime.Value.Month, q.t.ReadTime.Value.Day, q.t.ReadTime.Value.Hour, q.t.ReadTime.Value.Minute, q.t.ReadTime.Value.Second);
//                if (q.t.EndTime != null)
//                    q.t.EndTime = new DateTime(q.t.EndTime.Value.Year, q.t.EndTime.Value.Month, q.t.EndTime.Value.Day, q.t.EndTime.Value.Hour, q.t.EndTime.Value.Minute, q.t.EndTime.Value.Second);

//                tasks.Add(new TaskModel(q.a.FormUrl, q.t, q.e, q.p, q.unit.Title, q.pos.Title, q.a.Name, q.ProcessId, q.ra.Status, q.CreatorName));

//            }

//            var user = (from u in _repository.GetQuery<User>()
//                        where u.Id == userId
//                        select u).FirstOrDefault();
//            if (user != null) user.LastOnlineTime = DateTime.Now;

//            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

//            return tasks;

//        }

//        public async Task<TaskModel> StartTask(int taskId, int userId)
//        {
//            var now = DateTime.Now;

//            if (await _bpmsUnitOfWork.Tasks.AnyAsync(x => x.UserId == userId && x.Status == (short)TaskStatus.Inprogress))
//            {
//                return null;
//            }
//            else if (await _bpmsUnitOfWork.TaskWorkHistories.AnyAsync(x => x.TaskId == taskId && x.EndTime == null)) //Check there is any started record?
//                return null;
//            else
//            {
//                var task = await (from t in _bpmsUnitOfWork.Tasks
//                                  where (t.Id == taskId)
//                                  select t).FirstOrDefaultAsync();

//                task.StartTime = now;
//                task.Status = 3;

//                _bpmsUnitOfWork.TaskWorkHistories.Add(new TaskWorkHistory
//                {
//                    StartTime = now,
//                    Task = task
//                });

//                await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

//                //if (task.CreatorUserId == 10)
//                //    EndAutoTasks(taskId);

//                return new TaskModel(task);
//            }
//        }


//        public async Task<TaskModel> PauseTasks(int taskId)
//        {
//            var now = DateTime.Now;


//            var task = await (from t in _bpmsUnitOfWork.Tasks
//                              where (t.Id == taskId)
//                              select t).FirstOrDefaultAsync();


//            task.Status = 4;

//            var taskhistory = await (from th in _bpmsUnitOfWork.TaskWorkHistories
//                                     where (th.TaskId == task.Id) && (th.EndTime == null)
//                                     select th).FirstOrDefaultAsync();

//            taskhistory.EndTime = now;

//            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

//            return new TaskModel(task);

//        }

//        public async Task<TaskModel> ReadTasks(int taskId)
//        {
//            var now = DateTime.Now;


//            var task = await (from t in _bpmsUnitOfWork.Tasks
//                              where (t.Id == taskId)
//                              select t).FirstOrDefaultAsync();


//            if (task.Status < 2)
//            {
//                task.ReadTime = now;
//                task.Status = 2;
//            }

//            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
//            return new TaskModel(task);

//        }

//        public async Task<TaskModel> RejectTasks(int taskId)
//        {
//            var now = DateTime.Now;

//            var task = await (from t in _bpmsUnitOfWork.Tasks
//                              where (t.Id == taskId)
//                              select t).FirstOrDefaultAsync();

//            task.ReadTime = now;
//            task.Status = 6;

//            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
//            return new TaskModel(task);
//        }

//        public async Task<TaskModel> ContinueTasks(int taskId, int userId)
//        {
//            var now = DateTime.Now;

//            if (await _bpmsUnitOfWork.Tasks.AnyAsync(x => x.UserId == userId && x.Status == (short)TaskStatus.Inprogress))
//            {
//                return null;
//            }
//            else
//            {

//                var task = await (from t in _bpmsUnitOfWork.Tasks
//                                  where (t.Id == taskId)
//                                  select t).FirstOrDefaultAsync();

//                task.Status = 3;

//                _bpmsUnitOfWork.TaskWorkHistories.Add(new TaskWorkHistory
//                {
//                    StartTime = now,
//                    Task = task
//                });

//                await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
//                return new TaskModel(task);

//            }
//        }

//        public async Task<List<TaskModel>> GetHistoryTasks(int taskId)
//        {

//            var query = await (from th in _bpmsUnitOfWork.TaskWorkHistories
//                               where (th.TaskId == taskId)
//                               select new TaskModel
//                               {
//                                   StartTime = th.StartTime,
//                                   EndTime = th.EndTime,


//                               }).ToListAsync();

//            var now = DateTime.Now;
//            foreach (var q in query)
//            {
//                q.StartTimePersian = q.StartTime.ToPersianDateTime();
//                q.EndTimePersian = q.EndTime.ToPersianDateTime();
//                q.SpentTime = (int)((q.EndTime ?? DateTime.Now) - (DateTime)q.StartTime).TotalMinutes;
//                q.NowTime = now;//for prevent client toJson error
//                q.CreateTime = now;//for prevent client toJson error
//            }

//            return query;
//        }


//        public async Task<bool> EndTask(int taskId, int currentUserId, int primaryKeyValue,
//                     Dictionary<string, string> paramValues = null, bool activateNextActivity = true)
//        {
//            var now = DateTime.Now;

//            var taskQ = await (from t in _bpmsUnitOfWork.Tasks
//                               join ra in _bpmsUnitOfWork.RuntimeActivities on t.RuntimeActivityId equals ra.Id
//                               join a in _bpmsUnitOfWork.Activities on ra.ActivityId equals a.Id
//                               where (t.Id == taskId)
//                               select new { t, a.FinishedRule }).FirstOrDefaultAsync();
//            var task = taskQ.t;

//            task.EndTime = now;
//            if (task.StartTime != null)
//            {
//                var spend = (DateTime)task.EndTime - (DateTime)task.StartTime;
//                task.SpentTime = (int)spend.TotalMinutes;
//                task.Status = (short)TaskStatus.Reported;

//                if (task.Status != 4)
//                {
//                    var taskHistory = await (from th in _bpmsUnitOfWork.TaskWorkHistories
//                                             where (th.TaskId == task.Id) && (th.EndTime == null)
//                                             select th).FirstOrDefaultAsync();
//                    if (taskHistory != null)
//                        taskHistory.EndTime = now;
//                }


//                //-----------------------check whether finish activity of task or not----------------------
//                var finishActivity = false;
//                if (taskQ.FinishedRule == "Any")//finish activity of the task and other open susbset tasks of correspond acitivity
//                {
//                    finishActivity = true;
//                    //find other open tasks of the activity and end them
//                    var susbetTasks = await (from t in _bpmsUnitOfWork.Tasks
//                                             where t.RuntimeActivityId == task.RuntimeActivityId && t.Id != task.Id && t.Status < 5
//                                             select t).ToListAsync();
//                    foreach (var st in susbetTasks)
//                    {
//                        st.EndTime = task.EndTime;
//                        st.SpentTime = (int)spend.TotalMinutes;
//                        st.Status = (short)TaskStatus.Reported;
//                        st.Description = "(Activity FinishRule=Any)Finished By Task Id:" + task.Id;
//                    }

//                }
//                else//check is this task the last open task of the activity?
//                {
//                    var hasTasks = await (from t in _bpmsUnitOfWork.Tasks
//                                          where t.RuntimeActivityId == task.RuntimeActivityId && t.Id != task.Id && t.Status < 5
//                                          select t).AnyAsync();
//                    //if there is no other open task
//                    if (!hasTasks)
//                        finishActivity = true;
//                }

//                if (finishActivity)
//                {
//                    var rpa = await (from rp in _bpmsUnitOfWork.RuntimeProcesses
//                                     join ra in _bpmsUnitOfWork.RuntimeActivities on rp.Id equals ra.RuntimeProcessId
//                                     where ra.Id == task.RuntimeActivityId
//                                     select new { ra, rp }).FirstAsync();
//                    rpa.ra.IsFinished = true;
//                    rpa.ra.ProgressStatus = 100;
//                    rpa.ra.EndDate = task.EndTime;
//                    //if activateNextActivity=false,then we intend to pause the workflow
//                    // if (activateNextActivity)
//                    //  BpmsEngine.ActivateNextActivities(rpa.ra, task.Priority, currentUserId, primaryKeyValue, task.Title, paramValues);
//                }
//            }

//            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

//            return true;

//        }
//    }
//}
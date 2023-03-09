using System;
using Bpms.WorkflowEngine.Databases.Entities;

namespace Bpms.WorkflowEngine.Models
{
    public class TaskModel : Task
    {
        public TaskModel()
        {
            NowTime = DateTime.Now;
        }

        /*
        public TaskModel(string formUrl,Task task, Employee e, Person p, string unitName, string positionName,string activityName,int processId,short? status,string creatorName)
            : this()
        {
            Id = task.Id;
            FormUrl = formUrl;
            ActivityName = activityName;
            Title = task.Title;
            UserId = task.UserId;
            ActivityStatus = status;
            RuntimeWorkflowId = task.RuntimeWorkflowId;
            RuntimeActivityId = task.RuntimeActivityId;
            CreatorUserId = task.CreatorUserId;
            ActivityStatus = task.Status;
          
            CreateTime = task.CreateTime;
            ReadTime = task.ReadTime;
            StartTime = task.StartTime;
            EndTime = task.EndTime;
            NowTime = DateTime.Now;
            SpentTime = task.SpentTime;
            DateDue = task.DateDue;
            RelatedTaskId = task.RelatedTaskId;
            Description = task.Description;
            TimeoutDays = task.TimeoutDays;
            Priority = task.Priority;
            ProgressStatus = task.ProgressStatus;
            Status = task.Status;
            RelationType = task.RelationType;
            ProcessId = processId;
            if (e != null)
            {
                EmployeeCode = e.EmployeeCode;
                EmployeeId = e.Id;

            }

            if (p != null)
            {
                PersonCode = e.EmployeeCode;
                PersonName = p.FirstName + " " + p.LastName;
                PersonId = p.Id;
                CreatorName = creatorName;
            }

            UnitName = unitName;
            PositionName = positionName;

            //CreateTimePersian = task.CreateTime.ToPersianDateTime();
            //ReadTimePersian = task.ReadTime.ToPersianDateTime();
            //StartTimePersian = task.StartTime.ToPersianDateTime();
            //EndTimePersian = task.EndTime.ToPersianDateTime();
            //DateDuePersian = task.DateDue.ToPersianDateTime();
        }

        public TaskModel(Task task)
            : this("", task, null, null, "", "","",0,0,"")
        {
        }*/
        public string CreateTimePersian { get; set; }
        public string ReadTimePersian { get; set; }
        public string StartTimePersian { get; set; }
        public string EndTimePersian { get; set; }
        public string DateDuePersian { get; set; }

        public DateTime NowTime { get; set; }

        public string CreatorName { get; set; }

        public string PersonCode { get; set; }
        public int PersonId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }

        public decimal DueStatus { get; set; }
        public short? ActivityStatus { get; set; }

        
        public string PersonName { get; set; }
        public string PositionName { get; set; }
        public string UnitName { get; set; }
        public string ActivityName { get; set; }

        public string Moblie { get; set; }

        public int? WorkflowId { get; set; }

        public int ProcessId { get; set; }
        public string EnterTimePersian { get; set; }
        public string ExitTimePersian { get; set; }
        public bool IsOnline { get; set; }
        public string LastTimeOnline { get; set; }
        public new int Priority { get; set; }

        public string FormUrl { get; set; }
        

    }
}

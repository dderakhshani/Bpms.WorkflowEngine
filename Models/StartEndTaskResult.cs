using System.Collections.Generic;

namespace Bpms.WorkflowEngine.Models
{
    public class StartEndTaskResult
    {
        public StartEndTaskResult()
        {
            Tasks = new List<TaskModel>();
        }
        public int ResultType { get; set; }//1=Start 2=End
        public bool? Result { get; set; }
        public List<TaskModel> Tasks { get;  set; }
        //public Trouble Trouble { get; set; }
        public string EmployeeCode { get; set; }
    }
}

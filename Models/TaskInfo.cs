using Bpms.WorkflowEngine.Databases.Entities;

namespace Bpms.WorkflowEngine.Models
{
    public class TaskInfo : Task
    {
        public int TaskId { get; set; }
        public string ActivityName { get; set; }
        public int ActivityId { get; set; }
        public int WorkflowId { get; set; }
        public int ProcessId { get; set; }
        public int RuntimeProcessId { get; set; }   

    }
}

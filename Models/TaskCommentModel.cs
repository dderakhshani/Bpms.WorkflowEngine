using Bpms.WorkflowEngine.Databases.Entities;

namespace Bpms.WorkflowEngine.Models
{
    public class TaskCommentModel : TaskComment
    {
        public string CreateDatePersian { get; set; }
        public string CreatorName { get; set; }
        public int? StandRequestId { get; set; }
    }
}

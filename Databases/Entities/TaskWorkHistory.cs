using System;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class TaskWorkHistory : BaseEntity
    {
        public int TaskId { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public DateTime? EndTime { get; set; }

       


 
 
        public virtual Task Task { get; set; } = default!;
    }
}

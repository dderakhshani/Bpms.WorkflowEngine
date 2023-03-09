using System;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class TaskComment : BaseEntity
    {
        public int TaskId { get; set; } = default!;
        public string? Comment { get; set; }
        public DateTime? CreateDate { get; set; }

      


 
 
        public virtual Task Task { get; set; } = default!;
    }
}

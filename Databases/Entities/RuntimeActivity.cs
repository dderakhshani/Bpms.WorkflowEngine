using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class RuntimeActivity : BaseEntity
    {
        public int RuntimeProcessId { get; set; } = default!;
        public int? CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ActivityId { get; set; } = default!;
        public bool? IsFinished { get; set; }
        public short? ProgressStatus { get; set; }
        public short? Status { get; set; }

       
        public virtual Activity Activity { get; set; } = default!;
        public virtual RuntimeProcess RuntimeProcess { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
    }
}

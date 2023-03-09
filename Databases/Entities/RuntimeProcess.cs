using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class RuntimeProcess : BaseEntity
    {
        public int RuntimeWorkflowId { get; set; } = default!;
        public int? ParentRuntimeProcessId { get; set; }
        public int? Type { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProcessId { get; set; } = default!;


 
 
        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!;
        public virtual ICollection<RuntimeActivity> RuntimeActivities { get; set; } = default!;
    }
}

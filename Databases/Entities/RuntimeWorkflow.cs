using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class RuntimeWorkflow : BaseEntity
    {
        public int? CreatorId { get; set; }
        public int WorkflowId { get; set; } = default!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

       

        public virtual Workflow Workflow { get; set; } = default!;
        public virtual ICollection<RuntimeProcess> RuntimeProcesses { get; set; } = default!;
        public virtual ICollection<RuntimeWorkflowParameter> RuntimeWorkflowParameters { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
    }
}

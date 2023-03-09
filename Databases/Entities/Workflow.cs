using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Workflow : BaseEntity
    {
        public Guid Guid { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int? CreatedBy { get; set; }
        public int? DocumentId { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public string UniqueName { get; set; }
        public string WorkflowName { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }

        public virtual ICollection<Pool> Pools { get; set; } = default!;
        public virtual ICollection<Process> Processes { get; set; } = default!;
        public virtual ICollection<RuntimeWorkflow> RuntimeWorkflows { get; set; } = default!;
    }
}

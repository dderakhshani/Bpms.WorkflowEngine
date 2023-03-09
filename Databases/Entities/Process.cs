using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Process : BaseEntity
    {
        public Guid Guid { get; set; } = default!;
        public int WorkflowId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int? ParentProcessId { get; set; }
        public bool? IsActivitySet { get; set; }
        public int? ShapeId { get; set; }

        public virtual Shape? Shape { get; set; } = default!;
        public virtual Workflow Workflow { get; set; } = default!;
        public virtual ICollection<Activity> Activities { get; set; } = default!;
        public virtual ICollection<Transition> Transitions { get; set; } = default!;
    }
}

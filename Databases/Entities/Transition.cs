using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Transition : BaseEntity
    {
        public int ProcessId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Condition { get; set; }
        public string? Description { get; set; }
        public Guid? From { get; set; }
        public Guid? To { get; set; }
        public int? BusinessRuleId { get; set; }

        public virtual BusinessRule? BusinessRule { get; set; } = default!;
        public virtual Process Process { get; set; } = default!;
        public virtual ICollection<TransitionShape> TransitionShapes { get; set; } = default!;
    }
}

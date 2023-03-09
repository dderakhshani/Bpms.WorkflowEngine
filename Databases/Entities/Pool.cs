using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Pool : BaseEntity
    {
        public int WorkflowId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Type { get; set; }
        public bool? BoundaryVisible { get; set; }
        public int ShapeId { get; set; }


        public virtual Shape Shape { get; set; } = default!;
        public virtual Workflow Workflow { get; set; } = default!;
        public virtual ICollection<Lane> Lanes { get; set; } = default!;
    }
}

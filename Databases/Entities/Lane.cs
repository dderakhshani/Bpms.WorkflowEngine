using System;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Lane : BaseEntity
    {
        public int PoolId { get; set; } = default!;
        public Guid Guid { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int? PerentId { get; set; }
        public int ShapeId { get; set; }

        public virtual Shape Shape { get; set; } = default!;
        public virtual Pool Pool { get; set; } = default!;
    }
}

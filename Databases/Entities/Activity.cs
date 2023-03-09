using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Activity : BaseEntity
    {
        public Guid Guid { get; set; } = default!;
        public int? Type { get; set; }
        public string? Name { get; set; }
        public int? ProcessId { get; set; }
        public string? FormUrl { get; set; }

        /// <summary>
        /// Any=Finish the activity if any tasks of the activity finished,
        ///  All=Finish the activity if all tasks of the activity finished
        /// </summary>
        public string? FinishedRule { get; set; }
        public string? StatusDescription { get; set; }
        public short? DefaultStatus { get; set; }
        public int ShapeId { get; set; }

        public virtual Process? Process { get; set; } = default!;
        public virtual Shape Shape { get; set; } = default!;
        public virtual Event? Event { get; set; } = default!;
        public virtual ServiceActivity? ServiceActivity { get; set; } = default!;
        public virtual ICollection<Performer> Performers { get; set; } = default!;
        public virtual ICollection<RuntimeActivity> RuntimeActivities { get; set; } = default!;
    }
}

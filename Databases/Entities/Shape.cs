using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Shape : BaseEntity
    {
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? BorderColor { get; set; }
        public int? FillColor { get; set; }
        public bool? Expanded { get; set; }
        public int? ExpandedWidth { get; set; }
        public int? ExpandedHeight { get; set; }
        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

        public virtual Activity Activity { get; set; } = default!;
        public virtual Pool Pool { get; set; } = default!;
        public virtual Process Process { get; set; } = default!;
        public virtual Lane Lane { get; set; } = default!;
        public virtual ICollection<TransitionShape> TransitionShapes { get; set; } = default!;
    }
}
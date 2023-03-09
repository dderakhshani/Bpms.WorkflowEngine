using System;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Event : BaseEntity
    {
        public string? Trigger { get; set; }
        public bool? IsAttached { get; set; }
        public Guid? Target { get; set; }
     //   public int ActivityId { get; set; }
        /// <summary>
        /// Start = 1, Intermmediate = 2, End = 3
        /// </summary>
        public int Type { get; set; } = default!;



        public virtual Activity Activity { get; set; } = default!;
 
 
    }
}

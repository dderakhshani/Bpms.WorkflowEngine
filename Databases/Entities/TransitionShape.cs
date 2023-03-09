using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class TransitionShape : BaseEntity
    {
        public int TransitionId { get; set; }
        public int ShapeId { get; set; }

        public virtual Shape Shape { get; set; } = default!;
        public virtual Transition Transition { get; set; } = default!;
    }
}
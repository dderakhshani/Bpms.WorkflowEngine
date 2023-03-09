using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Enums;

namespace Bpms.WorkflowEngine.Models
{
   public class PossiblePerformerModel: PerformerCondition
    {
        public int NodeId { get; set; }
        public int ParentNodeId { get; set; }
        public int Level { get; set; }
        public ValueSourceType PerformerType { get; set; }
        public string Text { get; set; }
        public int? ActivityId { get; set; }
    }
}

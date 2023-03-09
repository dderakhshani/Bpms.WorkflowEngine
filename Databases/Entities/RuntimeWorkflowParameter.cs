using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class RuntimeWorkflowParameter : BaseEntity
    {
        public int RuntimeWorkflowId { get; set; } = default!;
        public string? ParameterName { get; set; }
        public string? ParameterValue { get; set; }
        public string? ParameterDataType { get; set; }

     


 
 
        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!;
    }
}

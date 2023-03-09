using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class ServiceActivity : BaseEntity
    {
        public string Url { get; set; } = default!;
        public string Parameters { get; set; } = default!;


        public virtual Activity IdNavigation { get; set; } = default!;
 
 
    }
}

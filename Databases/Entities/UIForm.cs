using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class UiForm : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Name { get; set; } = default!;


 
 
        public virtual ICollection<UiFormControl> UiFormControls { get; set; } = default!;
        public virtual ICollection<UiFormVariable> UiFormVariables { get; set; } = default!;
    }
}

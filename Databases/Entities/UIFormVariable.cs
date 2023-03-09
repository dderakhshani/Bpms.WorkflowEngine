using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class UiFormVariable : BaseEntity
    {
        public int UiFormId { get; set; } = default!;
        public string VariableName { get; set; } = default!;

        /// <summary>
        /// 1=int 2=String 3=Array
        /// </summary>
        public short Type { get; set; } = default!;

   

 
 
        public virtual UiForm UiForm { get; set; } = default!;
    }
}

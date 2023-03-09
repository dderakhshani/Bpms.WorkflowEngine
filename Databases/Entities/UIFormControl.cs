using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class UiFormControl : BaseEntity
    {
        public int FormId { get; set; } = default!;
        public int ParenId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int ControlId { get; set; } = default!;
        public bool IsReadOnly { get; set; } = default!;
        public bool IsVisible { get; set; } = default!;
        public bool IsRequired { get; set; } = default!;
        public string? DataSource { get; set; }
        public string? HelpText { get; set; }

    

        public virtual UiForm Form { get; set; } = default!;
 
 
        public virtual UiGrid? UiGrid { get; set; } = default!;
    }
}

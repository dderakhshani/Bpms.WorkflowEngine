using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class UiGridColumn : BaseEntity
    {
        public int GridId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Field { get; set; } = default!;
        public short Width { get; set; } = default!;
        public string Template { get; set; } = default!;
        public bool Searchable { get; set; } = default!;

      


        public virtual UiGrid Grid { get; set; } = default!;
 
 
    }
}

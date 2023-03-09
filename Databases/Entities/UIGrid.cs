using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class UiGrid : BaseEntity
    {
        public bool Searchable { get; set; } = default!;
        public bool ServerSideLoading { get; set; } = default!;
        public bool Filterable { get; set; } = default!;
        public bool Sortable { get; set; } = default!;
        public bool Groupable { get; set; } = default!;
        public string DataSource { get; set; } = default!;

    

        public virtual UiFormControl IdNavigation { get; set; } = default!;
 
 
        public virtual ICollection<UiGridColumn> UiGridColumns { get; set; } = default!;
    }
}

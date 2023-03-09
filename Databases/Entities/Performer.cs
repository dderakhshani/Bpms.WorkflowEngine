using System.Collections.Generic;
using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Performer : BaseEntity
    {
        public int ActivityId { get; set; } = default!;
        public string? Title { get; set; }

        /// <summary>
        /// 1=Everyone
        /// 2=by Load
        /// 3=Sequentional (Not supported yet)
        /// 4=First Aviliable User
        /// 
        /// </summary>
        public AssignationMethods AssignationMethod { get; set; }

        public virtual Activity Activity { get; set; } = default!;

 
 
        public virtual ICollection<PerformerCondition> PerformerConditions { get; set; } = default!;
    }
}

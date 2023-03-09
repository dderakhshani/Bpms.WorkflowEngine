

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class PerformerCondition : BaseEntity
    {
        public int? ParentId { get; set; }
        public int PerformerId { get; set; } = default!;
        public string StaticValue { get; set; }
        /// <summary>
        /// 1.Condition 2.And 3.Or 4....
        /// </summary>
        public PerformerConditionNodeType NodeType { get; set; }
        public PerformerTypes PerformerType { get; set; }
        /// <summary>
        /// &quot;Current&quot; value for to be equal to user correspond attribute or any static primary key value of correspond table
        /// </summary>
        public ValueSourceType ValueSourceType { get; set; }
        public bool? IsEqual { get; set; }
        public virtual Performer Performer { get; set; } = default!;
        public virtual PerformerCondition Parent { get; set; }

        [NotMapped]
        public virtual ICollection<PerformerCondition> Childs { get; set; }
    }
}

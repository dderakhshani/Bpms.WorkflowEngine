using System.Collections.Generic;
using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class BusinessRule : BaseEntity
    {
        public string RuleName { get; set; } = default!;

        /// <summary>
        /// 1=And 2=OR
        /// </summary>
        public CombinationTypes CombinationType { get; set; } = default!;
        public bool Invert { get; set; }
        public virtual ICollection<Transition> Transitions { get; set; }
        public virtual ICollection<BusinessRuleCondition> BusinessRuleConditions { get; set; } = default!;
    }

   
}

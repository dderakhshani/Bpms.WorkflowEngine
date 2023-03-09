using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class BusinessRuleCondition : BaseEntity
    {
        public int BusinessRuleId { get; set; } = default!;
        public string TableName { get; set; } = default!;
        public string FieldName { get; set; } = default!;

        /// <summary>
        /// 1:= 
        /// 2:&lt; 
        /// 3:&gt; 
        /// 4:&lt;= 
        /// 5:&gt;= 
        /// 6:in 
        /// 7:not in
        /// 8: Contains(Like) 9:&lt;&gt;
        /// </summary>
        public Comparison Operator { get; set; }
        public string Value { get; set; } = default!;
        public string? ForeignKeyName { get; set; }

    
        public virtual BusinessRule BusinessRule { get; set; } = default!;

 
 
    }
}

using System;
using System.Collections.Generic;
using Bpms.WorkflowEngine.Infrastructure;

namespace Bpms.WorkflowEngine.Databases.Entities
{
    public partial class Task : BaseEntity
    {
        public string? Title { get; set; }
        public int? RelatedTaskId { get; set; }
        public int RuntimeWorkflowId { get; set; } = default!;
        public int RuntimeActivityId { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public int CreatorUserId { get; set; } = default!;
        public DateTime CreateTime { get; set; } = default!;
        public DateTime? ReadTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// mintues
        /// </summary>
        public int? SpentTime { get; set; }
        public DateTime? DateDue { get; set; }
        public string? Description { get; set; }
        public short? TimeoutDays { get; set; }

        /// <summary>
        /// 1=Low 2=Normal 3=Major 4=Critical
        /// </summary>
        public int Priority { get; set; } = default!;
        public short ProgressStatus { get; set; } = default!;

        /// <summary>
        /// 1=Open 2=Read 3=Inprogress 4=Hold/Pause 5=Finished
        /// </summary>
        public short Status { get; set; } = default!;
        public int? RelationType { get; set; }
        public string? TempDesc { get; set; }


 
 
        public virtual RuntimeActivity RuntimeActivity { get; set; } = default!;
        public virtual RuntimeWorkflow RuntimeWorkflow { get; set; } = default!; 
        public virtual ICollection<TaskComment> TaskComments { get; set; } = default!;
        public virtual ICollection<TaskWorkHistory> TaskWorkHistories { get; set; } = default!;
    }
}

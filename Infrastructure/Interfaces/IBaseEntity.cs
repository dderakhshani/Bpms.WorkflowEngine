using System;

namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface IBaseEntity
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the OwnerRoleId
        /// </summary>
        public int OwnerRoleId { get; set; }
        /// <summary>
        /// Gets or sets the CreatorUserId
        /// </summary>
        public int CreatedById { get; set; }
        /// <summary>
        /// Gets or sets the addedDate
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the modifiedDate
        /// </summary>
        public int? ModifiedById { get; set; }

        /// <summary>
        /// Gets or sets the modifiedDate
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the isDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
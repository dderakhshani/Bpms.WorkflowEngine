using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;

namespace Bpms.WorkflowEngine.Infrastructure
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [IgnoreDataMember]
        public DateTime CreatedAt { get; set; }
        [IgnoreDataMember]
        public int? ModifiedById { get; set; }
        [IgnoreDataMember]
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        [IgnoreDataMember]
        public int OwnerRoleId { get; set; }
        [IgnoreDataMember]
        public int CreatedById { get; set; }
    }
}
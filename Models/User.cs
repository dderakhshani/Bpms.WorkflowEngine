using Bpms.WorkflowEngine.Infrastructure.Interfaces;

namespace Bpms.WorkflowEngine.Models
{
    public class User : IUser
    {
        public int UserId { get; set; }
        public int PositionId { get; set; }
        public int UnitId { get; set; }
        public int? ParentUnitId { get; set; }
        public int RoleId { get; set; }
        public int ParentRoleId { get; set; }
    }
}
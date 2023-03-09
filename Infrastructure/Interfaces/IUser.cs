namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface IUser
    {
        public int UserId { get; set; }
        public int PositionId { get; set; }
        public int UnitId { get; set; }
        public int? ParentUnitId { get; set; }
        public int RoleId { get; set; }
        public int ParentRoleId { get; set; }
    }
}
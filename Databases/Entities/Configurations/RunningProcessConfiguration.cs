using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class RuntimeProcessConfiguration : IEntityTypeConfiguration<RuntimeProcess>
    {
        public void Configure(EntityTypeBuilder<RuntimeProcess> entity)
        {
            entity.ToTable("RuntimeProcess", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.EndDate).HasColumnType("datetime");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.StartDate).HasColumnType("datetime");



            entity.HasOne(d => d.RuntimeWorkflow)
                .WithMany(p => p.RuntimeProcesses)
                .HasForeignKey(d => d.RuntimeWorkflowId)
                .HasConstraintName("FK_RuntimeProcess_RuntimeWorkflow");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RuntimeProcess> entity);
    }
}

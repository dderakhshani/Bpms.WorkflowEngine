using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class RuntimeActivityConfiguration : IEntityTypeConfiguration<RuntimeActivity>
    {
        public void Configure(EntityTypeBuilder<RuntimeActivity> entity)
        {
            entity.ToTable("RuntimeActivities", "Bpms.WorkflowEngine");

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

            entity.HasOne(d => d.Activity)
                .WithMany(p => p.RuntimeActivities)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK_RuntimeActivities_Activities");



            entity.HasOne(d => d.RuntimeProcess)
                .WithMany(p => p.RuntimeActivities)
                .HasForeignKey(d => d.RuntimeProcessId)
                .HasConstraintName("FK_RuntimeActivities_RuntimeProcess");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RuntimeActivity> entity);
    }
}

#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> entity)
        {
            entity.ToTable("Activities", "Bpms");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FinishedRule)
                .HasMaxLength(50)
                .HasComment("Any=Finish the activity if any tasks of the activity finished,\r\n All=Finish the activity if all tasks of the activity finished");

            entity.Property(e => e.FormUrl).HasMaxLength(150);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.Name).HasMaxLength(500);

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.StatusDescription).HasMaxLength(50);

            entity.HasOne(d => d.Process)
                .WithMany(p => p.Activities)
                .HasForeignKey(d => d.ProcessId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Activities_Processes");

            entity.HasOne(d => d.Shape)
                .WithOne(p => p.Activity)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Activity_Shapes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Activity> entity);
    }
}

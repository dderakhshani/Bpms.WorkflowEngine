using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class TaskWorkHistoryConfiguration : IEntityTypeConfiguration<TaskWorkHistory>
    {
        public void Configure(EntityTypeBuilder<TaskWorkHistory> entity)
        {
            entity.ToTable("TaskWorkHistory", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.EndTime).HasColumnType("datetime");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.StartTime).HasColumnType("datetime");


            entity.HasOne(d => d.Task)
                .WithMany(p => p.TaskWorkHistories)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_TaskWorkHistory_Tasks");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TaskWorkHistory> entity);
    }
}

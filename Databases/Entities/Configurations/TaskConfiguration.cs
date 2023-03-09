using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> entity)
        {
            entity.ToTable("Tasks", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DateDue).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.Property(e => e.EndTime).HasColumnType("datetime");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Priority).HasComment("1=Low 2=Normal 3=Major 4=Critical");

            entity.Property(e => e.ReadTime).HasColumnType("datetime");

            entity.Property(e => e.SpentTime).HasComment("mintues");

            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.Property(e => e.Status).HasComment("1=Open 2=Read 3=Inprogress 4=Hold/Pause 5=Finished");

            entity.Property(e => e.TempDesc).HasMaxLength(50);

            entity.Property(e => e.Title).HasMaxLength(100);


            entity.HasOne(d => d.RuntimeActivity)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.RuntimeActivityId)
                .HasConstraintName("FK_Tasks_RuntimeActivities");

            entity.HasOne(d => d.RuntimeWorkflow)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.RuntimeWorkflowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_RuntimeWorkflow1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Task> entity);
    }
}

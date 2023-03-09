using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class ProcessConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> entity)
        {
            entity.ToTable("Processes", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");



            entity.HasOne(d => d.Workflow)
                .WithMany(p => p.Processes)
                .HasForeignKey(d => d.WorkflowId)
                .HasConstraintName("FK_Processes_Workflows");

            entity.HasOne(d => d.Shape)
                .WithOne(p => p.Process)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Process_Shapes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Process> entity);
    }
}

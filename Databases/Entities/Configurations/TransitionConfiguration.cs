using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class TransitionConfiguration : IEntityTypeConfiguration<Transition>
    {
        public void Configure(EntityTypeBuilder<Transition> entity)
        {
            entity.ToTable("Transitions", "Bpms.WorkflowEngine");

            entity.Property(e => e.Condition).HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Description).HasMaxLength(100);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.HasOne(d => d.Process)
                .WithMany(p => p.Transitions)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK_Transitions_Processes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Transition> entity);
    }
}

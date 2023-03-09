using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class RuntimeWorkflowParameterConfiguration : IEntityTypeConfiguration<RuntimeWorkflowParameter>
    {
        public void Configure(EntityTypeBuilder<RuntimeWorkflowParameter> entity)
        {
            entity.ToTable("RuntimeWorkflowParameter", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ParameterDataType).HasMaxLength(20);

            entity.Property(e => e.ParameterName).HasMaxLength(50);

            entity.Property(e => e.ParameterValue).HasMaxLength(50);


            entity.HasOne(d => d.RuntimeWorkflow)
                .WithMany(p => p.RuntimeWorkflowParameters)
                .HasForeignKey(d => d.RuntimeWorkflowId)
                .HasConstraintName("FK_RuntimeWorkflowParameter_RuntimeWorkflow");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<RuntimeWorkflowParameter> entity);
    }
}

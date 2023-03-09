using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class UiFormVariableConfiguration : IEntityTypeConfiguration<UiFormVariable>
    {
        public void Configure(EntityTypeBuilder<UiFormVariable> entity)
        {
            entity.ToTable("UIFormVariables", "Bpms.WorkflowEngine");

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

            entity.Property(e => e.Type).HasComment("1=int 2=String 3=Array");

            entity.Property(e => e.VariableName)
                .IsRequired()
                .HasMaxLength(50);


            entity.HasOne(d => d.UiForm)
                .WithMany(p => p.UiFormVariables)
                .HasForeignKey(d => d.UiFormId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UIFormVariables_UIForms");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UiFormVariable> entity);
    }
}

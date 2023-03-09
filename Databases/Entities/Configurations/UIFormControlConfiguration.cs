using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class UiFormControlConfiguration : IEntityTypeConfiguration<UiFormControl>
    {
        public void Configure(EntityTypeBuilder<UiFormControl> entity)
        {
            entity.ToTable("UIFormControls", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DataSource).HasMaxLength(50);

            entity.Property(e => e.HelpText).HasMaxLength(400);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");


            entity.HasOne(d => d.Form)
                .WithMany(p => p.UiFormControls)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UIFormControls_UIForms");



            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UiFormControl> entity);
    }
}

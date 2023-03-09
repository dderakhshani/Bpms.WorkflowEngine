using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class UiGridColumnConfiguration : IEntityTypeConfiguration<UiGridColumn>
    {
        public void Configure(EntityTypeBuilder<UiGridColumn> entity)
        {
            entity.ToTable("UIGridColumns", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.Field)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Template)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);


            entity.HasOne(d => d.Grid)
                .WithMany(p => p.UiGridColumns)
                .HasForeignKey(d => d.GridId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UIGridColumns_UIGrid");


            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UiGridColumn> entity);
    }
}

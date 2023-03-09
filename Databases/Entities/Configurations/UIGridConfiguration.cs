using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class UiGridConfiguration : IEntityTypeConfiguration<UiGrid>
    {
        public void Configure(EntityTypeBuilder<UiGrid> entity)
        {
            entity.ToTable("UIGrid", "Bpms.WorkflowEngine");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.DataSource)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");


            entity.HasOne(d => d.IdNavigation)
                .WithOne(p => p.UiGrid)
                .HasForeignKey<UiGrid>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UIGrid_UIFormControls");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UiGrid> entity);
    }
}

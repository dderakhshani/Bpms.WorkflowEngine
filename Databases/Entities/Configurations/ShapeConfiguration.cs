using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class ShapeConfiguration : IEntityTypeConfiguration<Shape>
    {
        public void Configure(EntityTypeBuilder<Shape> entity)
        {
            entity.ToTable("Shapes", "Bpms.WorkflowEngine");

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

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Shape> entity);
    }
}
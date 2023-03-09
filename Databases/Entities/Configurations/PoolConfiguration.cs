using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class PoolConfiguration : IEntityTypeConfiguration<Pool>
    {
        public void Configure(EntityTypeBuilder<Pool> entity)
        {
            entity.ToTable("Pools", "Bpms.WorkflowEngine");

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
                .HasMaxLength(50)
                .IsFixedLength(true);

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsFixedLength(true);


            entity.HasOne(d => d.Workflow)
                .WithMany(p => p.Pools)
                .HasForeignKey(d => d.WorkflowId)
                .HasConstraintName("FK_Pools_Workflows");

            entity.HasOne(d => d.Shape)
                .WithOne(p => p.Pool)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Pool_Shapes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Pool> entity);
    }
}

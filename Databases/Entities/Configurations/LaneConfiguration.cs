using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class LaneConfiguration : IEntityTypeConfiguration<Lane>
    {
        public void Configure(EntityTypeBuilder<Lane> entity)
        {
            entity.ToTable("Lanes", "Bpms.WorkflowEngine");

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

            entity.HasOne(d => d.Pool)
                .WithMany(p => p.Lanes)
                .HasForeignKey(d => d.PoolId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Lanes_Pools");


            entity.HasOne(d => d.Shape)
                .WithOne(p => p.Lane)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Lane_Shapes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Lane> entity);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class ServiceActivityConfiguration : IEntityTypeConfiguration<ServiceActivity>
    {
        public void Configure(EntityTypeBuilder<ServiceActivity> entity)
        {
            entity.ToTable("ServiceActivities", "Bpms.WorkflowEngine");

            entity.Property(e => e.Id).ValueGeneratedNever();

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

            entity.Property(e => e.Parameters)
                .IsRequired()
                .HasMaxLength(400);

            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(150);


            entity.HasOne(d => d.IdNavigation)
                .WithOne(p => p.ServiceActivity)
                .HasForeignKey<ServiceActivity>(d => d.Id)
                .HasConstraintName("FK_ServiceActivities_Activities");


            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ServiceActivity> entity);
    }
}

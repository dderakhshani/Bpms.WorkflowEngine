using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            entity.ToTable("Events", "Bpms.WorkflowEngine");

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

            entity.Property(e => e.Trigger).HasMaxLength(50);

            entity.Property(e => e.Type).HasComment("Start = 1, Intermmediate = 2, End = 3");


            entity.HasOne(d => d.Activity)
                .WithOne(p => p.Event)
                .HasForeignKey<Event>(d => d.Id)
                .HasConstraintName("FK_Events_Activities");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Event> entity);
    }
}

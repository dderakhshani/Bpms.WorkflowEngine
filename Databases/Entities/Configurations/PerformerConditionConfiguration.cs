using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class PerformerConditionConfiguration : IEntityTypeConfiguration<PerformerCondition>
    {
        public void Configure(EntityTypeBuilder<PerformerCondition> entity)
        {
            entity.ToTable("PerformerCondition", "Bpms.WorkflowEngine");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.PerformerType);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.NodeType).HasComment("1.Condition 2.And 3.Or 4....");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.ValueSourceType)
                .HasComment("\"Current\" value for to be equal to user correspond attribute or any static primary key value of correspond table");


            entity.HasOne(d => d.Performer)
                .WithMany(p => p.PerformerConditions)
                .HasForeignKey(d => d.PerformerId)
                .HasConstraintName("FK_PerformerCondition_Performers");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PerformerCondition> entity);
    }
}

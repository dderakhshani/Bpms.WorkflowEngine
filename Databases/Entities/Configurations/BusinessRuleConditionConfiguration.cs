using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#nullable disable

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class BusinessRuleConditionConfiguration : IEntityTypeConfiguration<BusinessRuleCondition>
    {
        const string Default_Schema = "bpms";
        public void Configure(EntityTypeBuilder<BusinessRuleCondition> entity)
        {
            entity.ToTable("BusinessRuleConditions", Default_Schema);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان ایجاد");

            entity.Property(e => e.CreatedById).HasComment("ایجاد کننده");

            entity.Property(e => e.FieldName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ForeignKeyName).HasMaxLength(50);

            entity.Property(e => e.IsDeleted).HasComment("آیا حذف شده است؟");

            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("تاریخ و زمان اصلاح");

            entity.Property(e => e.ModifiedById).HasComment("اصلاح کننده");

            entity.Property(e => e.Operator).HasComment("1:= \r\n2:< \r\n3:> \r\n4:<= \r\n5:>= \r\n6:in \r\n7:not in\r\n8: Contains(Like) 9:<>");

            entity.Property(e => e.OwnerRoleId).HasComment("نقش صاحب سند");

            entity.Property(e => e.TableName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.BusinessRule)
                .WithMany(p => p.BusinessRuleConditions)
                .HasForeignKey(d => d.BusinessRuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BusinessRuleConditions_BusinessRules");


            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<BusinessRuleCondition> entity);
    }
}

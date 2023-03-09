using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bpms.WorkflowEngine.Databases.Entities.Configurations
{
    public partial class TransitionShapeConfiguration : IEntityTypeConfiguration<TransitionShape>
    {
        public void Configure(EntityTypeBuilder<TransitionShape> entity)
        {
            entity.ToTable("TransitionShapes", "Bpms.WorkflowEngine");

            entity.Property(e => e.ShapeId);
            entity.Property(e => e.TransitionId);

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


            entity.HasOne(d => d.Shape)
                .WithMany(p => p.TransitionShapes)
                .HasForeignKey(d => d.ShapeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransitionShapes_Shape");    
            
            entity.HasOne(d => d.Transition)
                .WithMany(p => p.TransitionShapes)
                .HasForeignKey(d => d.TransitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransitionShapes_Transition");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TransitionShape> entity);
    }
}
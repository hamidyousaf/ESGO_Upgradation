﻿namespace Infrastructure.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Description)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder
         .Property(b => b.CreatedDate)
         .HasDefaultValueSql("GETUTCDATE()")
         .IsRequired();
        builder
            .Property(x => x.UpdatedDate)
            .IsRequired(false);
        #endregion

        #region Global query filter
        builder.HasQueryFilter(x => !x.IsDeleted && x.IsActive);
        #endregion
    }
}

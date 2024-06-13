namespace Infrastructure.Configurations;

internal class EmployementConfiguration : IEntityTypeConfiguration<Employement>
{
    public void Configure(EntityTypeBuilder<Employement> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        builder
            .Property(x => x.CompanyName)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.CompanyAddress)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.StartDate)
            .IsRequired();
        builder
            .Property(x => x.EndDate)
            .IsRequired();
        builder
            .Property(x => x.Position)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.ReasonForLeaving)
            .HasMaxLength(255)
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

namespace Infrastructure.Configurations;

public class EmployerContactDetailConfiguration : IEntityTypeConfiguration<EmployerContactDetail>
{
    public void Configure(EntityTypeBuilder<EmployerContactDetail> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employer)
            .WithMany(x => x.EmployerContactDetails)
            .HasForeignKey(b => b.EmployerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder
            .Property(x => x.ContactName)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasMaxLength(62)
            .IsRequired();
        builder
            .Property(x => x.CountryCode)
            .HasMaxLength(5)
            .IsRequired();
        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .IsRequired();
        builder
            .Property(x => x.JobTitle)
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

namespace Infrastructure.Configurations;

public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
{
    public void Configure(EntityTypeBuilder<Employer> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Email)
            .HasMaxLength(62)
            .IsRequired();
        builder
            .Property(x => x.JobTitle)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Name)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.CompanyName)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.CompanyNo)
            .HasMaxLength(12)
            .IsRequired();
        builder
            .Property(x => x.CompanyTypeId)
            .IsRequired();
        builder
            .Property(x => x.AboutOrganization)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.OrganizationLogoUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.OrganizationImageUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.SiteName)
            .HasMaxLength(50)
            .IsRequired();
        builder
            .Property(x => x.PinCode)
            .HasMaxLength(7)
            .IsRequired();
        builder
            .Property(x => x.NearestLocation)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Location)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Address)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Address2)
            .HasMaxLength(255)
            .IsRequired(false);
        builder
            .Property(x => x.IsHealthAndSafetyPolicy)
            .IsRequired();
        builder
            .Property(x => x.AccountStatus)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.SelfCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.PayrollCommission)
            .HasDefaultValue(0)
            .IsRequired();
        builder
            .Property(x => x.LimitedCommission)
            .HasDefaultValue(0)
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

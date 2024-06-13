namespace Infrastructure.Configurations;

public class ReferenceConfiguration : IEntityTypeConfiguration<Reference>
{
    public void Configure(EntityTypeBuilder<Reference> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .HasDefaultValue("")
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasDefaultValue("")
            .HasMaxLength(62)
            .IsRequired();
        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Relationship)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.CharacterProfile)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.OrganizationName)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.OrganizationEmail)
            .HasMaxLength(62)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.OrganizationPhoneNumber)
            .HasDefaultValue("")
            .HasMaxLength(15)
            .IsRequired();
        builder
            .Property(x => x.JobTitle)
            .HasDefaultValue("")
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.Position)
            .HasDefaultValue("")
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.BothWork)
            .HasDefaultValue("")
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(x => x.StartDate)
            .IsRequired();
        builder
            .Property(x => x.EndDate)
            .IsRequired();
        builder
            .Property(x => x.CharacterDescription)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.ReferenceTypeId)
            .HasDefaultValue(ReferenceTypeEnum.Personal)
            .IsRequired();
        builder
            .Property(x => x.Status)
            .HasDefaultValue(PersonalReferenceStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.RejectionReason)
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

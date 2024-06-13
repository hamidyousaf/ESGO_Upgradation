namespace Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        #region Entity configuration
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Title)
            .HasMaxLength(5)
            .IsRequired();
        builder
            .Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        builder
            .Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();
        builder
            .Property(x => x.MaritalStatus)
            .HasMaxLength(15)
            .IsRequired();
        builder
            .Property(x => x.CountryCode)
            .HasDefaultValueSql("44")
            .HasMaxLength(5)
            .IsRequired();
        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(11)
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasMaxLength(62)
            .IsRequired();
        builder
            .Property(x => x.EmployementTypeId)
            .IsRequired();
        builder
            .Property(x => x.UTRNumber)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.UTRNumberStatus)
            .HasDefaultValue(UTRNumberStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.UTRNumberRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.CompanyNumber)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.CompanyNumberStatus)
            .HasDefaultValue(CompanyNumberStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.CompanyNumberRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.BiometricResidenceCardUrl)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.BiometricResidenceCardStatus)
            .HasDefaultValue(BiometricResidenceCardStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.BiometricResidenceCardRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.PassportUrl)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.PassportStatus)
            .HasDefaultValue(PassportStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.PassportRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.DbsNumberStatus)
            .HasDefaultValue(DbsNumebrStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.DbsNumberRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.DbsCertificateStatus)
            .HasDefaultValue(DbsCertificateStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.DbsCertificateRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceNumber)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceNumberStatus)
            .HasDefaultValue(NationalInsuranceNumberStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceNumberRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.ProfileImageUrl)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.P45DocumentUrl)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.EmployeeTypeId)
            .IsRequired();
        builder
            .Property(x => x.PinCode)
            .HasMaxLength(15)
            .IsRequired();
        builder
            .Property(x => x.CVFileURL)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.ProofOfExemptionUrl)
            .HasMaxLength(255)
            .IsRequired(false);
        builder
            .Property(x => x.VaccinationCertificateUrl)
            .HasMaxLength(255)
            .IsRequired(false);
        builder
            .Property(x => x.NoOfShifts)
            .HasDefaultValue((byte)0)
            .IsRequired(false);
        builder
            .Property(x => x.DateOfQualification)
            .IsRequired(false);
        builder
            .Property(x => x.NurseTypeId)
            .IsRequired(false);
        builder
            .Property(x => x.NMCPin)
            .HasMaxLength(10)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NMCPinRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NMCPinStatus)
            .HasDefaultValue(NMCPinStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.InterviewStatus)
            .HasDefaultValue(InterviewStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.InterviewFileUrl)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.InterviewRemarks)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.YearsOfExperience)
            .HasDefaultValue((byte)0)
            .IsRequired(false);
        builder
            .Property(x => x.Address)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Address2)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.City)
            .HasMaxLength(150)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.Country)
            .HasMaxLength(150)
            .HasDefaultValue("England")
            .IsRequired(false);
        builder
            .Property(x => x.AccountStatus)
            .HasDefaultValue(EmployeeAccountStatusEnum.Pending)
            .IsRequired();
        builder
            .Property(x => x.AccountStatusChangeReason)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.DbsCertificateUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.DbsNumber)
            .HasMaxLength(15)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.AccountName)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.AccountNumber)
            .HasMaxLength(30)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.SortCode)
            .HasMaxLength(10)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.BankName)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.DateOfBirth)
            .IsRequired();
        builder
            .Property(x => x.Gender)
            .HasDefaultValue(GenderEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.Nationality)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.HaveDbsNumber)
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .Property(x => x.DbsExpiryDate)
            .IsRequired(false);
        builder
            .Property(x => x.Policy1)
            .IsRequired();
        builder
            .Property(x => x.Policy2)
            .IsRequired();
        builder
            .Property(x => x.Policy3)
            .IsRequired();
        builder
            .Property(x => x.Policy4)
            .IsRequired();
        builder
            .Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        builder
            .Property(x => x.HaveQualification)
            .HasDefaultValue(false)
            .IsRequired(false);
        builder
            .Property(x => x.IsSubjectOfInvestigation)
            .HasDefaultValue(false)
            .IsRequired(false);
        builder
            .Property(x => x.WorkGapReason)
            .HasDefaultValue("")
            .IsRequired(false);
        builder
            .Property(x => x.PersonalLink)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired(false);
        builder
            .Property(x => x.ProfessionalLink)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired(false);
        builder
            .Property(x => x.EmployeeSecretId)
            .IsRequired();
        builder
            .Property(x => x.LinkSharedOnEmails)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.AccessNIUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.AccessNIExpiryDate)
            .IsRequired(false);
        builder
            .Property(x => x.AccessNIStatus)
            .HasDefaultValue(AccessNIStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.AccessNIRejectionReason)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceUrl)
            .HasMaxLength(255)
            .HasDefaultValue("")
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceStatus)
            .HasDefaultValue(NationalInsuranceStatusEnum.Unknown)
            .IsRequired();
        builder
            .Property(x => x.NationalInsuranceRejectionReason)
            .HasDefaultValue("")
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

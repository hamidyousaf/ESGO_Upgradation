namespace Domain.DTOs.Responces;

public class SignUpAPIsResponse
{
    public List<EmployeeTypeResponse> EmployeeTypes { get; set; }
    public List<EmployementTypeResponse> EmployementTypes { get; set; }
}

public class EmployeeTypeResponse
{
    public byte Id { get; set; }
    public string Name { get; set; }
}
public class EmployementTypeResponse
{
    public byte Id { get; set; }
    public string Name { get; set; }
}

public class GetQualificationsByEmployeeResponse
{
    public int Id { get; set; }
    public string Course { get; set; }
    public DateTime DateOfAward { get; set; }
    public string AwardingBody { get; set; }
}


public class GetEmployementsByEmployeeResponse
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Position { get; set; }
    public string ReasonForLeaving { get; set; }
}

public class GetEmployeeBySecretIdResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid EmployeeSecretId { get; set; }
    public bool CanAddReference { get; set; }
}

public class CanAddPersonalReferenceResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid EmployeeSecretId { get; set; }
}

public class CanAddProfessionalReferenceResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid EmployeeSecretId { get; set; }
}

public class GetEmployeeDocumentsResponse
{
    public byte DocumentId { get; set; }
    public int? EmployeeDocumentId { get; set; }
    public string DocumentName { get; set; }
    public string? DocumentUrl { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public byte? Status { get; set; }
    public string Reason { get; set; }
}


public class GetDocumentsByEmployeeResponse
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public byte DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentUrl { get; set; }
    public DateTime UploadedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Reason { get; set; }
    public byte Status { get; set; }
}

public class ResultForGetDocumentsByEmployeeResponse
{
    public int TotalDocuments { get; set; }
    public int TotalUploadedDocuments { get; set; }
    public List<GetDocumentsByEmployeeResponse> Documents { get; set; }
}
public class GetDocumentByCategoryIdResponse
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public byte CategoryId { get; set; }
}

public class GetDocumentPolicyInfoResponse
{
    public int EmployeeId { get; set; }
    public bool Policy1 { get; set; }
    public bool Policy2 { get; set; }
    public bool Policy3 { get; set; }
    public bool Policy4 { get; set; }
}

public class GetEmployeeByIdResponse
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MaritalStatus { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string ProfileImageUrl { get; set; }
    public byte EmployementTypeId { get; set; }
    public byte EmployeeTypeId { get; set; }
    public string PinCode { get; set; }
    public string UTRNumber { get; set; }
    public byte UTRNumberStatus { get; set; }
    public string UTRNumberRejectionReason { get; set; }
    public string? CVFileURL { get; set; }
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public byte AccountStatus { get; set; }
    public string AccountStatusChangeReason { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ProofOfExemptionUrl { get; set; }
    public string? VaccinationCertificateUrl { get; set; }
    public byte? NoOfShifts { get; set; }
    public DateTime? DateOfQualification { get; set; }
    public byte? NurseTypeId { get; set; }
    public string? NMCPin { get; set; }
    public byte? NMCPinStatus { get; set; }
    public string? NMCPinRejectionReason { get; set; }
    public byte? YearsOfExperience { get; set; }
    public bool? HaveQualification { get; set; }
    public bool? IsSubjectOfInvestigation { get; set; }
    public string? WorkGapReason { get; set; }
    public string? PersonalLink { get; set; }
    public string? ProfessionalLink { get; set; }
    public Guid EmployeeSecretId { get; set; }
    public string LinkSharedOnEmails { get; set; }
    public string DbsCertificateUrl { get; set; }
    public DateTime? DbsExpiryDate { get; set; }
    public string DbsNumber { get; set; }
    public bool HaveDbsNumber { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string SortCode { get; set; }
    public string BankName { get; set; }
    public bool Policy1 { get; set; }
    public bool Policy2 { get; set; }
    public bool Policy3 { get; set; }
    public bool Policy4 { get; set; }
    public string CompanyNumber { get; set; }
    public byte CompanyNumberStatus { get; set; }
    public string CompanyNumberRejectionReason { get; set; }
    public EmployeeTypeResponce EmployeeType { get; set; }
    public string BiometricResidenceCardUrl { get; set; }
    public byte BiometricResidenceCardStatus { get; set; }
    public string BiometricResidenceCardRejectionReason { get; set; }
    public string NationalInsuranceNumber { get; set; }
    public byte NationalInsuranceNumberStatus { get; set; }
    public string NationalInsuranceNumberRejectionReason { get; set; }
    public string P45DocumentUrl { get; set; }
    public string AccessNIUrl { get; set; }
    public DateTime? AccessNIExpiryDate { get; set; }
    public byte AccessNIStatus { get; set; }
    public string AccessNIRejectionReason { get; set; }
    public string PassportUrl { get; set; }
    public byte PassportStatus { get; set; }
    public string PassportRejectionReason { get; set; }
    public DateTime DateOfBirth { get; set; }
    public byte Gender { get; set; }
    public string Nationality { get; set; }
    public byte DbsNumberStatus { get; set; }
    public string DbsNumberRejectionReason { get; set; }
    public byte DbsCertificateStatus { get; set; }
    public string DbsCertificateRejectionReason { get; set; }
    public string NationalInsuranceUrl { get; set; }
    public byte NationalInsuranceStatus { get; set; }
    public string NationalInsuranceRejectionReason { get; set; }
    public bool IsRegistered { get; set; }
    public bool IsAddressAdded { get; set; }
    public bool IsNoOfShiftsAdded { get; set; }
    public bool IsNMCAdded { get; set; }
    public bool IsStarterFormAdded { get; set; }
    public bool IsQualificationAdded { get; set; }
    public bool IsEmployementAdded { get; set; }
    public bool IsReferenceAdded { get; set; }
    public bool IsCertificateAdded { get; set; }
    public bool IsRightToWorkAdded { get; set; }
    public bool IsDBSAdded { get; set; }
    public bool IsBankDetailsAdded { get; set; }
    public bool IsDocumentedAdded { get; set; }
    public bool IsAccessNIAdded { get; set; }
    public GetPersonalReferenceByEmployeeIdResponce? PersonalReference { get; set; }
    public GetProfessionalReferenceByEmployeeIdResponce? ProfessionalReference { get; set; }
    public List<DbsDocumentDto> DbsDocuments { get; set; }
}

public class DbsDocumentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string GroupNo { get; set; }
    public byte DocumentNumber { get; set; }
    public string Url { get; set; }
    public byte Status { get; set; }
    public string? RejectionReason { get; set; }
}

public class EmployeeTypeResponce
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MinRate { get; set; }
}

public class GetStarterFormAnswerResponse
{
    public int QuestionId { get; set; }
    public bool YesOrNo { get; set; }
}

public class GetEmployeeShortDetailsByIdResponce
{
    public int Id { get; set; }
    public string ProfileImageUrl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string EmployeeType { get; set; }
    public string EmployementType { get; set; }
    public string Address { get; set; }
    public string PinCode { get; set; }
    public byte AccountStatus { get; set; }
}
public class GetEmployeeAllDetailsByIdResponce
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MaritalStatus { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public EmployementTypeForGetEmployeeAllDetailsByIdResponce EmployementType { get; set; }
    public EmployeeTypeForGetEmployeeAllDetailsByIdResponce EmployeeType { get; set; }
    public string PinCode { get; set; }
    public string CompanyNumber { get; set; }
    public byte CompanyNumberStatus { get; set; }
    public string CompanyNumberRejectionReason { get; set; }
    public byte? YearsOfExperience { get; set; }
    public string UTRNumber { get; set; }
    public byte UTRNumberStatus { get; set; }
    public string ProfileImageUrl { get; set; }
    public string BiometricResidenceCardUrl { get; set; }
    public byte BiometricResidenceCardStatus { get; set; }
    public string BiometricResidenceCardRejectionReason { get; set; }
    public string PassportUrl { get; set; }
    public byte PassportStatus { get; set; }
    public string PassportRejectionReason { get; set; }
    public byte DbsNumberStatus { get; set; }
    public string DbsNumberRejectionReason { get; set; }
    public byte DbsCertificateStatus { get; set; }
    public string DbsCertificateRejectionReason { get; set; }
    public string P45DocumentUrl { get; set; }
    public string UTRNumberRejectionReason { get; set; }
    public string? CVFileURL { get; set; }
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public byte AccountStatus { get; set; }
    public string AccountStatusChangeReason { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ProofOfExemptionUrl { get; set; }
    public string? VaccinationCertificateUrl { get; set; }
    public byte? NoOfShifts { get; set; }
    public DateTime? DateOfQualification { get; set; }
    public byte? NurseTypeId { get; set; }
    [NotMapped]
    public NurseTypeEnum NT
    {
        get => (NurseTypeEnum)NurseTypeId;
        set => NurseTypeId = (byte)value;
    }
    public string NurseType { get => NT.ToString(); }
    public string NMCPin { get; set; }
    public byte NMCPinStatus { get; set; }
    public string NMCPinRejectionReason { get; set; }
    public string NationalInsuranceNumber { get; set; }
    public byte NationalInsuranceNumberStatus { get; set; }
    public string NationalInsuranceNumberRejectionReason { get; set; }
    public bool? HaveQualification { get; set; }
    public bool? IsSubjectOfInvestigation { get; set; }
    public string? WorkGapReason { get; set; }
    public string? PersonalLink { get; set; }
    public string? ProfessionalLink { get; set; }
    public string DbsCertificateUrl { get; set; }
    public DateTime? DbsExpiryDate { get; set; }
    public string DbsNumber { get; set; }
    public bool HaveDbsNumber { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string SortCode { get; set; }
    public string BankName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public byte Gender { get; set; }
    [NotMapped]
    public GenderEnum G
    {
        get => (GenderEnum)Gender;
        set => Gender = (byte)value;
    }
    public string GenderDEscription { get => G.ToString(); }
    public string Nationality { get; set; }
    public bool Policy1 { get; set; }
    public bool Policy2 { get; set; }
    public bool Policy3 { get; set; }
    public bool Policy4 { get; set; }
    public GetPersonalReferenceByEmployeeIdResponce? PersonalReference { get; set; }
    public GetProfessionalReferenceByEmployeeIdResponce? ProfessionalReference { get; set; }
    public List<GetQualificationsByEmployeeResponse> Qualifications { get; set; }
    public List<GetEmployementsByEmployeeResponse> Employements { get; set; }
    public List<GetDocumentsByEmployeeResponse> TrainingCertificates { get; set; }
    public List<StarterFormQuestionsResponce> StarterFormQuestions { get; set; }
    public List<DbsDocumentDto> DbsDocuments { get; set; }
    public string[] EmployerFeedbacks { get; set; }
    public string NationalInsuranceUrl { get; set; }
    public byte NationalInsuranceStatus { get; set; }
    public string NationalInsuranceRejectionReason { get; set; }
    [NotMapped]
    public InterviewStatusEnum IS
    {
        get => (InterviewStatusEnum)InterviewStatus;
        set => InterviewStatus = (byte)value;
    }
    public string InterviewStatusDescription { get => IS.ToString(); }
    public byte InterviewStatus { get; set; }
    public string InterviewRemarks { get; set; }
    public string InterviewFileUrl { get; set; }
}
public class GetEmployerAllDetailsByIdResponce
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string? Address2 { get; set; }
    public string CompanyName { get; set; }
    public string CompanyNo { get; set; }
    public byte CompanyTypeId { get; set; } // Use CompanyTypeEnum
    [NotMapped]
    public CompanyTypeEnum CT
    {
        get => (CompanyTypeEnum)CompanyTypeId;
        set => CompanyTypeId = (byte)value;
    }
    public string CompanyType { get => CT.ToString(); }
    public string SiteName { get; set; }
    public string PinCode { get; set; }
    public string AboutOrganization { get; set; }
    public string[]? TypeOfService { get; set; }
    public List<ContactDetailDto> ContactDetails { get; set; }

}
public class ContactDetailDto
{
    public string ContactName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CountryCode { get; set; }
}

public class EmployementTypeForGetEmployeeAllDetailsByIdResponce
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
public class EmployeeTypeForGetEmployeeAllDetailsByIdResponce
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MinRate { get; set; }
}
public class StarterFormQuestionsResponce
{
    public int Id { get; set; }
    public string Question { get; set; }
    public bool YesOrNo { get; set; }
}

public class GetAssignedJobsResponse
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public byte EmployeeTypeId { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetUrgentJobsResponse
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string ShiftTime { get; set; }
    public bool IsDummy { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetAllJobsResponse
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetFavouriteJobsResponse
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetAppliedJobsResponse
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public byte EmployeeTypeId { get; set; }
    public decimal HourlyRate { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsSelected { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetConfirmedJobsResponse
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public byte EmployeeTypeId { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetAssignedJobByIdResponce
{
    public int AssignedJobId { get; set; }
    public int EmployerId { get; set; }
    public string EmployeeTypeDescription { get; set; }
    public EmployerForGetAssignedJobByIdResponce Employer { get; set; }
    public JobForGetAssignedJobByIdResponce Job { get; set; }
}

public class GetJobByIdResponce
{
    public int JobId { get; set; }
    public string EmployeeTypeDescription { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string CompanyNo { get; set; }
    public string PinCode { get; set; }
    public string AboutOrganization { get; set; }
    public string OrganizationImageUrl { get; set; }
    public string Address { get; set; }
    public string? Address2 { get; set; }
    public bool IsFavourite { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Date { get; set; }
    public DateTime ShiftStartTime { get; set; }
    public DateTime ShiftEndTime { get; set; }
    public decimal HourlyRate { get; set; }
    public byte BreakTime { get; set; }
    public decimal CostPershift { get; set; }
    public decimal JobHoursPerDay { get; set; }
    public string JobDescription { get; set; }
    public bool IsDummy { get; set; }
    public int EmployerId { get; set; }
    public byte EmployeeTypeId { get; set; }
}

public class GetTimesheetByIdResponce
{
    public int Id { get; set; }
    public string OrganisationName { get; set; }
    public DateTime Date { get; set; }  
    public DateTime StartTime { get; set; } 
    public DateTime EndTime { get; set; }   
    public decimal HourlyRate { get; set; }     
    public byte BreakTime { get; set; } 
    public TimeOnly BillableHours { get; set; }
    public byte Status { get; set; } // Enum: TimeSheetStatus
    public string? Notes { get; set; }
    public string? Reason{ get; set; }
}

public class GetPendingJobCountsResponce
{
    public int TotalAppliedJobs { get; set; }
    public int TotalAssignedJobs { get; set; }
    public int TotalConfirmedJobs { get; set; }
}

public class GetTimesheetCountsResponce
{
    public int TotalPendingTimesheets { get; set; }
    public int TotalApprovedTimesheets { get; set; }
    public int TotalRejectedTimesheets { get; set; }
}

public class GetJobCountsResponce
{
    public int TotalAllJobs { get; set; }
    public int TotalFavouriteJobs { get; set; }
    public int TotalUrgentJobs { get; set; }
}

public class EmployerForGetAssignedJobByIdResponce
{
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string CompanyNo { get; set; }
    public string PinCode { get; set; }
    public string AboutOrganization { get; set; }
    public string OrganizationImageUrl { get; set; }
    public string Address { get; set; }
    public string? Address2 { get; set; }
    public bool IsFavourite { get; set; }
    public string PhoneNumber { get; set; }
}
public class JobForGetAssignedJobByIdResponce
{
    public DateTime Date { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public decimal HourlyRate { get; set; }
    public byte BreakTime { get; set; }
    public decimal CostPershift { get; set; }
    public decimal JobHoursPerDay { get; set; }
    public string JobDescription { get; set; }
    public bool IsDummy { get; set; }
}
public class GetDocumentTypesResponce
{
    public List<DocumentTypeForGetDocumentTypesResponce> DocumentOneType { get; set; }
    public List<DocumentTypeForGetDocumentTypesResponce> DocumentTwoType { get; set; }
    public List<DocumentTypeForGetDocumentTypesResponce> DocumentThreeType { get; set; }
}

public class DocumentTypeForGetDocumentTypesResponce
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string GroupNo { get; set; }
}

public class GetAssignedJobByEncryptIdResponce
{
    public int AssignedJobId { get; set; }
    public int JobId { get; set; }
    public string CompanyName { get; set; }
    public DateTime JobDate { get; set; }
    public string Shift { get; set; }
    public byte Status { get; set; }
}

public class GetEmployeeAllDetailsByIdForEmployeeResponce
{
    public string FirstName { get; set; }
    public string ProfileImageUrl { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PinCode { get; set; }
    public string EmployementTypeDescription { get; set; }
    public int ProfilePercentage { get; set; }
    public string BiometricResidenceCardUrl { get; set; }
    public string PassportUrl { get; set; }
    public string P45DocumentUrl { get; set; }
    public string AccessNIUrl { get; set; }
    public decimal AmountCredited { get; set; }
    public decimal AmountPending { get; set; }
    public decimal TotalHours { get; set; }
    public decimal TotalJobs { get; set; }
    public decimal ActiveJobs { get; set; }
    public decimal AppliedJobs { get; set; }
    public decimal RejectedJobs { get; set; }
    public decimal CancelledJobs { get; set; }
    public List<EmployementDto> Experiences { get; set; }
}
public class EmployementDto
{
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class HomeJobSearchResponce
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsDummy { get; set; }
}

public class PendingJobSearchResponce
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsSelected { get; set; }
}

public class CompletedJobSearchResponce
{
    public int AssignedJobId { get; set; }
    public DateTime StartDate { get; set; }
    public string Location { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public string EmployeeType { get; set; }
    public string PinCode { get; set; }
    public decimal HourlyRate { get; set; }
    public string Status { get; set; }
    public byte JobStatus { get; set; }
    public bool IsUrgent { get; set; }
}

public class GetOpenJobsForHomePageResponce
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsDummy { get; set; }
    public DateTime CreatedDate { get; set;}
    public string Location { get; set; }
    public byte EmployeeTypeId { get; set; }
}

public class WebGuestJobSearchResponce
{
    public int JobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsDummy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Location { get; set; }
    public byte EmployeeTypeId { get; set; }

}

public class GetCalenderJobsResponce
{
    public List<JobForGetCalenderJobsResponce> AppliedJobs { get; set; }
    public List<JobForGetCalenderJobsResponce> AssignedJobs { get; set; }
    public List<JobForGetCalenderJobsResponce> ConfirmedJobs { get; set; }
}

public class JobForGetCalenderJobsResponce
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public decimal HourlyRate { get; set; }
    public string ShiftTime { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsSelected { get; set; }
}
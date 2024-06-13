namespace Domain.DTOs.Requests;

public class AddEmployeeRequest
{
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Title field is required."), MaxLength(5)]
    public string Title { get; set; }
    [Required(ErrorMessage = "First name field is required."), MaxLength(55)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last name field is required."), MaxLength(55)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Marital status field is required."), MaxLength(15)]
    public string MaritalStatus { get; set; }
    [Required(ErrorMessage = "Country code field is required."), MaxLength(5)]
    public string CountryCode { get; set; }
    [Required(ErrorMessage = "Phone number field is required."), MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Email field is required."), MaxLength(55), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Employee type field is required.")]
    public byte EmployeeTypeId { get; set; }
    [Required(ErrorMessage = "Postal code field is required."), MaxLength(7)]
    public string PinCode { get; set; }
    [Required(ErrorMessage = "Employement type field is required.")]
    public byte EmployementTypeId { get; set; }
    public string? CVFileURL { get; set; }
    [Required(ErrorMessage = "Latitude field is required.")]
    public double Latitude { get; set; }
    [Required(ErrorMessage = "Longitude field is required.")]
    public double Longitude { get; set; }
    public string? Country { get; set; }
}
public class UpdateTimeSheetRequest
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int EmployerId { get; set; }
    public DateTime? WorkDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public byte? BreakTime { get; set; }
    public DateTime? BillableHours { get; set; }
    public decimal? TotalHours { get; set; }
    public decimal? HoursRate { get; set; }
    public decimal? TotalAmount { get; set; }
    public byte Status { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public decimal? PlateformFess { get; set; }
    public decimal? FinalAmount { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? UploadedTime { get; set; }
    public string? StatusChangedBy { get; set; }
    public decimal? BillableHourInDecimal { get; set; }
    public decimal? OrginalTotalAmount { get; set; }
    public decimal? OriginalHourlyRate { get; set; }
    public decimal? TotalHolidayAmount { get; set; }
}

public class ChangeEmployeeStatusRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    public string? Reason { get; set; } = string.Empty;
}

public class AddAddressRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Address field is required.")]
    public string Address { get; set; }
    public string? Address2 { get; set; } = string.Empty;
    [Required(ErrorMessage = "City field is required.")]
    public string City { get; set; }
    [Required(ErrorMessage = "PinCode field is required.")]
    public string PinCode { get; set; }
    [Required(ErrorMessage = "Latitude field is required.")]
    public double Latitude { get; set; }
    [Required(ErrorMessage = "Longitude field is required.")]
    public double Longitude { get; set; }
    [Required(ErrorMessage = "Country field is required.")]
    public string Country { get; set; }
}

public class AddVaccinationRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
    [Required(ErrorMessage = "IsVaccinated field is required.")]
    public bool IsVaccinated { get; set; }
}

public class AddShiftsRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "No Of Shifts field is required."), Range(0.0, byte.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public byte NoOfShifts { get; set; }
}

public class AddNMCRegistrationRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Date of qualification field is required.")]
    public DateTime DateOfQualification { get; set; }
    [Required(ErrorMessage = "Nurse type field is required.")]
    public byte NurseTypeId { get; set; }
    [Required(ErrorMessage = "NMC Pin field is required."), MaxLength(10)]
    public string NMCPin { get; set; }
    public byte? YearsOfExperience { get; set; } = 0;
}

public class AddQualificationRequest
{
    [Required(ErrorMessage = "Employee id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Course field is required."), MaxLength(255)]
    public string Course { get; set; }
    [Required(ErrorMessage = "Date of award field is required.")]
    public DateTime DateOfAward { get; set; }
    [Required(ErrorMessage = "Awarding body field is required."), MaxLength(255)]
    public string AwardingBody { get; set; }
}

public class DeleteQualificationRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Qualification id field is required.")]
    public int QualificationId { get; set; }
}

public class DeleteDbsDocumentRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Document Id field is required.")]
    public int DocumentId { get; set; }
}

public class AddHaveQualificationRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Qualification field is required.")]
    public bool HaveQualification { get; set; }
}

public class AddIsSubjectOfInvestigationRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Is subject of investigation field is required.")]
    public bool IsSubjectOfInvestigation { get; set; }
}

public class AddEmployementRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Company name field is required."), MaxLength(255)]
    public string CompanyName { get; set; }
    [Required(ErrorMessage = "Company address field is required."), MaxLength(255)]
    public string CompanyAddress { get; set; }
    [Required(ErrorMessage = "Position field is required."), MaxLength(255)]
    public string Position { get; set; }
    [Required(ErrorMessage = "Start date field is required.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End date field is required.")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "End date field is required."), MaxLength(255)]
    public string ReasonForLeaving { get; set; }
}

public class DeleteEmployementRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Employement id field is required.")]
    public int EmployementId { get; set; }
}

public class AddWorkGapReasonRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Reason field is required."), MaxLength(512)]
    public string Reason { get; set; }
}

public class SendReferenceLinkToMailRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Reference Type field is required.")]
    public byte ReferenceType { get; set; }
    [Required(ErrorMessage = "Email field is required."), EmailAddress]
    public string Email { get; set; }
}

public class AddPersonalReferenceRequest
{
    [Required(ErrorMessage = "Employee secret id field is required.")]
    public Guid EmployeeSecretId { get; set; }
    [Required(ErrorMessage = "Name field is required."), MaxLength(255)]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email field is required."), MaxLength(62), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Phone number field is required."), MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Character profile field is required.")]
    public string CharacterProfile { get; set; }
}

public class AddProfessionalReferenceRequest
{
    [Required(ErrorMessage = "Employee secret id field is required.")]
    public Guid EmployeeSecretId { get; set; }
    [Required(ErrorMessage = "Organization name field is required."), MaxLength(255)]
    public string OrganizationName { get; set; }
    [Required(ErrorMessage = "Organization email field is required."), MaxLength(62), EmailAddress]
    public string OrganizationEmail { get; set; }
    [Required(ErrorMessage = "Organization phone number field is required."), MaxLength(15)]
    public string OrganizationPhoneNumber { get; set; }
    [Required(ErrorMessage = "Job title field is required."), MaxLength(255)]
    public string JobTitle { get; set; }
    [Required(ErrorMessage = "Position field is required."), MaxLength(255)]
    public string Position { get; set; }
    [Required(ErrorMessage = "Both work field is required."), MaxLength(255)]
    public string BothWork { get; set; }
    [Required(ErrorMessage = "Start date field is required.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End date field is required.")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "TillDate field is required.")]
    public bool TillDate { get; set; }
    [Required(ErrorMessage = "Character description field is required.")]
    public string CharacterDescription { get; set; }
}

public class AddEmployeeDocumentRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Document id field is required.")]
    public byte DocumentId { get; set; }
    [Required(ErrorMessage = "Expiry Date field is required.")]
    public DateTime ExpiryDate { get; set; }
    [Required(ErrorMessage = "Document field is required.")]
    public IFormFile Document { get; set; }
}

public class DeleteEmployeeDocumentRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Employee Document id field is required.")]
    public int EmployeeDocumentId { get; set; }
}

public class AddDbsCertificateRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
    [Required(ErrorMessage = "Expiry Date field is required.")]
    public DateTime ExpiryDate { get; set; }
}

public class AddNationalInsuranceRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddAccessNIRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
    [Required(ErrorMessage = "Expiry Date field is required.")]
    public DateTime ExpiryDate { get; set; }
}

public class AddDbsNumberRequest
{
    public int? EmployeeId { get; set; }
    [MaxLength(15)]
    public string? DbsNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "National Insurance number field is required.")]
    public string NationalInsuranceNumber { get; set; }
    [Required(ErrorMessage = "Have Dbs number field is required.")]
    public bool HaveDbsNumber { get; set; }
}

public class AddBankDetailsRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Account name field is required.")]
    [MaxLength(255)]
    public string AccountName { get; set; }
    [Required(ErrorMessage = "Account number field is required.")]
    [MaxLength(30)]
    public string AccountNumber { get; set; }
    [Required(ErrorMessage = "Sort code field is required.")]
    [MaxLength(10)]
    public string SortCode { get; set; }
    [Required(ErrorMessage = "Bank name field is required.")]
    [MaxLength(255)]
    public string BankName { get; set; }
}

public class AddDocumentPolicyStatusRequest
{
    public int? EmployeeId { get; set; }
    [Required(ErrorMessage = "Policy1 field is required.")]
    public bool Policy1 { get; set; }
    [Required(ErrorMessage = "Policy2 field is required.")]
    public bool Policy2 { get; set; }
    [Required(ErrorMessage = "Policy3 field is required.")]
    public bool Policy3 { get; set; }
    [Required(ErrorMessage = "Policy4 field is required.")]
    public bool Policy4 { get; set; }
}

public class ChangeEmployerStatusRequest
{
    [Required(ErrorMessage = "Employer id field is required.")]
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
}

public class UpdateEmployeeRequest
{
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Title field is required.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "First Name field is required.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name field is required.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Marital Status field is required.")]
    public string MaritalStatus { get; set; }
    [Required(ErrorMessage = "Email field is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Employee Type field is required.")]
    public byte EmployeeTypeId { get; set; }
    [Required]
    public byte EmployementTypeId { get; set; }
    public string? UTRNumber { get; set; } = string.Empty;
    public string? CompanyNumber { get; set; } = string.Empty;
}

public class AddProfileImageRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddCVRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddP45DocumentRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddBiometricResidenceCardRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddPassportRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddRightToWorkRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "Date Of Birth field is required.")]
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "Gender field is required.")]
    public byte Gender { get; set; }
    [Required(ErrorMessage = "Nationality field is required."), MaxLength(255)]
    public string Nationality { get; set; }
}

public class SubmitTimesheetRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required]
    public int TimesheetId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    [Required]
    public decimal HourlyRate { get; set; }
    public string? Notes { get; set; } = string.Empty;
}

public class AddStarterFormRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required, DisplayName("National Insurance Number"), MaxLength(25)]
    public string NationalInsuranceNumber { get; set; }
    public List<QuestionAnswer> QuestionAnswers { get; set; }
}
public class QuestionAnswer
{
    [Required(ErrorMessage = "QuestionId field is required.")]
    public int QuestionId { get; set; }
    [Required(ErrorMessage = "YesOrNo field is required.")]
    public bool YesOrNo { get; set; }
}

public class AddDbsDocumentsRequest
{
    public int EmployeeId { get; set; } = 0;
    [Required(ErrorMessage = "Document Number field is required.")]
    public byte DocumentNumber { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
    [Required(ErrorMessage = "Document type id field is required.")]
    public int DocumentTypeId { get; set; }
}

public class ApplyJobRequest
{
    [Required(ErrorMessage = "JobId field is required.")]
    public int JobId { get; set; }
}

public class HomeJobSearchRequest
{
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public string PostalCode { get; set; }
    public byte Radius { get; set; }
    public string TabName { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class PendingJobSearchRequest
{
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public string PostalCode { get; set; }
    public byte Radius { get; set; }
    public string TabName { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class CompletedJobSearchRequest
{
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public string PostalCode { get; set; }
    public byte Radius { get; set; }
    public string TabName { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetOpenJobsForHomePageRequest
{
    public byte EmployeeTypeId { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class WebGuestJobSearchRequest
{
    public byte EmployeeTypeId { get; set; }
    public DateTime FromDate { get; set; }
    public decimal MinRate { get; set; }
    public string? PostalCode { get; set; } = string.Empty;
    public byte Radius { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetCalenderJobsRequest
{
    public int EmployeeId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class AddToFavouriteRequest
{
    public int EmployeeId { get; set; }
    [Required]
    public int EmployerId { get; set; }
}
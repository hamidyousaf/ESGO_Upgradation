namespace Domain.DTOs.Responces;

public class GetAllEmployeesByStatusResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EmployeeType { get; set; }
    public string? PinCode { get; set; }
    public string? City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Reason { get; set; }
    public string CVFileURL { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class AdminLoginResponse
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class GetAllDocumentsByEmployeeResponse
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public byte DocumentId { get; set; }
    public string DocumentUrl { get; set; }
    public DateTime UploadedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public byte Status { get; set; }
}

public class GetAllEmployersByStatusResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string SiteName { get; set; }
    public string Address { get; set; }
    public byte AccountStatus { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class GetAllBookingsResponce
{
    public int Id { get; set; }
    public string EmployerName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int EmployerId { get; set; }
    public string Details { get; set; }
    public byte Status { get; set; }
    public string DocumentUrl { get; set; }
}

public class GetBookingResponce
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int EmployerId { get; set; }
    public string Details { get; set; }
    public byte Status { get; set; }
    public string DocumentUrl { get; set; }
}

public class GetEmployeeTypesResponce
{
    public byte Id { get; set; }
    public string Name { get; set; }
}

public class GetEmployeeTypeByIdResponse
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MinRate { get; set; }
}

public class GetEmployeesByEmployeeTypeIdResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public byte AccountStatus { get; set; }
}

public class GetShiftsByEmployerIdResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
}

public class GetJobsByStatusResponse
{
    public int AssignedJobId { get; set; }
    public int TimesheetId { get; set; }
    public int JobId { get; set; }
    public DateTime StartDate { get; set; }
    public string Location { get; set; }
    public DateTime ShiftStartTime { get; set; }
    public DateTime ShiftEndTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployeeType { get; set; }
    public byte EmployeeTypeId { get; set; }
    public string EmployeeName { get; set; }
    public int EmployeeId { get; set; }
    public byte JobStatus { get; set; }
    public byte? Status { get; set; }
    public TimeOnly? BillableHours { get; set; }
    public string? Notes { get; set; }
    public byte? Rating { get; set; }
    public string ReviewedBy { get; set; }
    public string ShiftDescription { get; set; }
    public int ShiftId { get; set; }
    public int EmployerId { get; set; }
    public string OrganisationName { get; set; }
    public decimal HourlyRate { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime JobCreatedDate { get; set; }
    public string CancellationReason { get; set; }
    public byte BreakTime { get; set; }
    public byte JobTypeId { get; set; }
    public int Waiting { get; set; }
    public int Applied { get; set; }
    public List<string> AppliedEmployees { get; set; }
    public List<string> WaitingEmployees { get; set; }
    public List<string> CancelledEmployees { get; set; }
    public int Canceled { get; set; }
    public int Applicant { get; set; }
    public bool IsDummy { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public int Confirmed { get; set; }
    [NotMapped]
    public JobTypeEnum JT
    {
        get => (JobTypeEnum)JobTypeId;
        set => JobTypeId = (byte)value;
    }
    public string JobType { get => JT.ToString(); }
}

public class GetShadowShiftsResponse
{
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class GetMonthlySupervisionReportsResponse
{
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class GetFeedBacksResponse
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class GetDbsExpiredEmployeesResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FullName { get; set; }
    public string MaritalStatus { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class GetTrainingCertificateExpiredEmployeesResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FullName { get; set; }
    public string MaritalStatus { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
}

public class GetUnsuccessfulJobsResponse
{
    public int AssignedJobId { get; set; }
    public DateTime StartDate { get; set; }
    public string Location { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public string EmployeeType { get; set; }
}

public class GetConfirmedJobsForAdminResponse
{
    public int AssignedJobId { get; set; }
    public DateTime StartDate { get; set; }
    public string Location { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public string EmployeeType { get; set; }
}

public class GetCompletedJobsResponse
{
    public int AssignedJobId { get; set; }
    public DateTime Date { get; set; }
    public byte EmployeeTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public string Location { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public string EmployeeType { get; set; }
    public string PinCode { get; set; }
    public decimal HourlyRate { get; set; }
    public string Status { get; set; }
    public byte JobStatus { get; set; }
    public bool IsUrgent { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal HourlyRateAfterSelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal HourlyRateAfterPayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    public decimal HourlyRateAfterLimitedCommission { get; set; }
}

public class GetTimesheetsByStatusResponse
{
    public int TimesheetId { get; set; }
    public string OrganisationName { get; set; }
    public string PostalCode { get; set; }
    public DateTime Date { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
    public decimal FixedRateAfterCommission { get; set; }
    public decimal HourlyRate { get; set; }
    public TimeOnly ShiftStartTime { get; set; }
    public TimeOnly ShiftEndTime { get; set; }
    public byte Status { get; set; }
    public bool IsUrgent { get; set; }
}

public class GetAssignedJobEmployeeByIdForAdminResponse
{
    public int AssignedJobId { get; set; }
    public string EmployeeName { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime AppliedDate { get; set; }
}

public class GetEmployersForInvoiceResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompanyName { get; set; }
}

public class GetTotalAmountOfEmployerResponse
{
    public decimal Amount { get; set; }
}

public class GetNextInvoiceNumberResponse
{
    public int NextInvoiceNumber { get; set; }
}

public class GetNextBookingIdResponse
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}

public class GetEmployerByIdResponse
{
    public string OrganizationImageUrl { get; set; }
    public string OrganizationLogoUrl { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string JobTitle { get; set; }
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
    public string Location { get; set; }
    public string Address { get; set; }
    public string? Address2 { get; set; }
    public string AboutOrganization { get; set; }
    public List<byte> Services { get; set; }
    public List<GetEmployerByIdContactDetailDto> ContactDetails { get; set; }
}

public class GetAPIsForJobResponce
{
    public int Id { get; set; }
    public string EmployerName { get; set; }
    public int EmployerId { get; set; }
    public List<EmployeeTypeDto> EmployeeTypes { get; set; }
    public List<JobTypeDto> JobTypes { get; set; }
    public List<EmployeeCategoryDto> EmployeeCategories { get; set; }
    public List<ShiftDto> Shifts { get; set; }
    public List<byte> BreakTime { get; set; }
}
public class GetEmployeesByEmployeeTypeIdAndEmployeeCategoryIdResponce
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class GetDashboardDetailsResponce
{
    public int TotalEmployees { get; set; }
    public int TotalActiveEmployees { get; set; }
    public int TotalInActiveEmployees { get; set; }
    public int TotalPendingEmployees { get; set; }
    public int TotalEmployers { get; set; }
    public int TotalActiveEmployers { get; set; }
    public int TotalInActiveEmployers { get; set; }
    public int TotalPendingEmployers { get; set; }
    public List<EmployementTypeForDashboard> EmployementTypes { get; set; }
    public List<EmployeeTypeForDashboard> EmployeeTypes { get; set; }
    public List<JobsForDashboard> JobsForDashboard { get; set; }
}
public class JobsForDashboard
{
    public int AssignedJobId { get; set; }
    public DateTime Date { get; set; }
    public string EmployerName { get; set; }
    public string ShiftDescription { get; set; }
    public decimal HourlyRate { get; set; }
    public int ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public byte Status { get; set; }
}
public class EmployementTypeForDashboard
{
    public byte EmployementTypeId { get; set; }
    public string Name { get; set; }
    public int Total { get; set; }
}
public class EmployeeTypeForDashboard
{
    public byte EmployeeTypeId { get; set; }
    public string Name { get; set; }
    public int Total { get; set; }
}
public class GetTimesheetsByStatusForAdminResponce
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int EmployeeId { get; set; }
    public int EmployerId { get; set; }
    public string EmployeeType { get; set; }
    public int ShiftId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployeeName { get; set; }
    public string EmployerName { get; set; }
    public string ShiftDescription { get; set; }
    public TimeOnly BillableHours { get; set; }
    public byte BreakTime { get; set; }
    public TimeOnly TotalHours { get; set; }
    public string? Notes { get; set; } = string.Empty;
    public string ReviewedBy { get; set; }
    public decimal HourlyRate { get; set; } // from Job entity.
    public byte Status { get; set; }
}

public class GetTimesheetByIdForAdminResponce
{
    public int Id { get; set; }
    public string OrganisationName { get; set; }
    public string EmployeeName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime TimesheetDate { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeOnly TotalHours { get; set; }
    public byte BreakTime { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalAmount { get; set; }
    public byte Status { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? RejectionDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public TimeOnly BillableHours { get; set; }
    public string? Reason { get; set; }
}

public class GetAssignedJobByIdForAdminResponce
{
    public int AssignedJobId { get; set; }
    public string OrganisationName { get; set; }
    public decimal HourlyRate { get; set; }
    public string EmployeeType { get; set; }
    public DateTime JobCreatedDate { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string JobDescription { get; set; }
}

public class GetJobHistoryByEmployeeIdResponce
{
    public int AssignedJobId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployerName { get; set; }
    public string ShiftDescription { get; set; }
    public decimal HourlyRate { get; set; }
    public byte Status { get; set; }
}
public class GetEmployeeByAssignedJobIdResponce
{
    public int AssignedJobId { get; set; }
    public int JobId { get; set; }
    public string EmployeeName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime AppliedDate { get; set; }
}
public class GetEmployeeJobDetailsByIdResponce
{
    public int AssignedJobId { get; set; }
    public DateTime JobDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public string OrganisationName { get; set; }
    public decimal HourlyRate { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public DateTime SelectionDate { get; set; }
    public DateTime CanellationDate { get; set; }
    public DateTime? TimesheetDate { get; set; }
    public int ShiftId { get; set; }
}
public class GetEmployerJobsResponce
{
    public int AssignedJobId { get; set; }
    public DateTime Date { get; set; }
    public int ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployerName { get; set; }
    public string ShiftDescription { get; set; }
    public string EmployeeType { get; set; }
    public decimal HourlyRate { get; set; }
    public byte Status { get; set; }
    public byte SelectedSource { get; set; }
    public byte Count { get; set; }
    public DateTime CancellationDate { get; set; }
}

public class GetEmployeesByJobIdResponce
{
    public int JobId { get; set; }
    public int AssignedJobId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime AppliedDate { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public byte Status { get; set; }
    [NotMapped]
    public JobStatusEnum JS
    {
        get => (JobStatusEnum)Status;
        set => Status = (byte)value;
    }
    public string StatusDescription { get => JS.ToString(); }
}

public class EmployeeLoginResponse
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public string Country { get; set; }
    public byte EmployementTypeId { get; set; }
    public byte EmployeeTypeId { get; set; }
    public byte AccountStatus { get; set; }
}
public class GetNotificationsResponce
{
    public int TotalUnread { get; set; }
    public List<Notification> Notifications { get; set; } = new();
}
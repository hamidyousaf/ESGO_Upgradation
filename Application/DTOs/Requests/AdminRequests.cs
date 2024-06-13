namespace Application.DTOs.Requests;

public class ChangeEmployeeDocumentStatusRequest
{
    public int EmployeeId { get; set; }
    public int EmployeeDocumentId { get; set; }
    public byte Status { get; set; }
    public string? Reason { get; set; } = string.Empty;
}

public class AddJobCommissionRequest
{
    [Required(ErrorMessage = "Employer id field is required.")]
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Self Commission field is required.")]
    public decimal SelfCommission { get; set; } = 0;
    [Required(ErrorMessage = "Payroll Commission field is required.")]
    public decimal PayrollCommission { get; set; } = 0;
    [Required(ErrorMessage = "Limited Commission field is required.")]
    public decimal LimitedCommission { get; set; } = 0;
}

public class ChangeBookingStatusRequest
{
    [Required(ErrorMessage = "Booking id field is required.")]
    public int BookingId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class AddJobRequest
{
    [Required(ErrorMessage = "Employer Id field is required.")]
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Employee Type Id field is required.")]
    public byte EmployeeTypeId { get; set; }
    [Required(ErrorMessage = "Job Type Id field is required.")]
    public byte JobTypeId { get; set; }
    public int? BookingId { get; set; }
    [Required(ErrorMessage = "Start Date field is required.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End Date field is required.")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Shift Id field is required.")]
    public int ShiftId { get; set; }
    [Required(ErrorMessage = "Shift Start Time field is required.")]
    public string ShiftStartTime { get; set; }
    [Required(ErrorMessage = "Shift End Time field is required.")]
    public string ShiftEndTime { get; set; }
    [Required(ErrorMessage = "Hourly Rate field is required.")]
    public decimal HourlyRate { get; set; }
    [Required(ErrorMessage = "Break Time field is required.")]
    public byte BreakTime { get; set; }
    [Required(ErrorMessage = "Cost Per shift field is required.")]
    public decimal CostPershift { get; set; }
    [Required(ErrorMessage = "Employer Category Id field is required.")]
    public byte EmployeeCategoryId { get; set; }
    [Required(ErrorMessage = "Job Description field is required.")]
    public string JobDescription { get; set; }
    [Required(ErrorMessage = "EmployeeIds field is required.")]
    public int[] EmployeeIds { get; set; }
    public bool IsDummy { get; set; }
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
}

public class ChangeUTRNumberStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeCompanyNumberStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeNMCRegistrationStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeNationalInsuranceNumberStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangePersonalReferenceStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeProfessionalReferenceStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeBiometricResidenceCardStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangePassportStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeDbsDocumentStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "DocumentId field is required.")]
    public int DocumentId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeDbsNumebrStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeDbsCertificateStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeAccessNIStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class ChangeNationalInsuranceStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [MaxLength(1024)]
    public string? Reason { get; set; } = string.Empty;
}

public class AddShadowShiftRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
}

public class AddInterviewRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public string Remarks { get; set; }
}

public class DeleteEmployeesByIdsRequest
{
    [Required(ErrorMessage = "Employee Ids field is required.")]
    public int[] EmployeeIds { get; set; }
}

public class DeleteEmployersByIdsRequest
{
    [Required(ErrorMessage = "Employer Ids field is required.")]
    public int[] EmployerIds { get; set; }
}

public class AddMonthlySupervisionReportRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }

    [Required(ErrorMessage = "Date field is required.")]
    public DateTime Date { get; set; }
}

public class ChangeInterviewStatusRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "File field is required.")]
    public IFormFile File { get; set; }    
    [Required(ErrorMessage = "Remarks field is required.")]
    public string Remarks { get; set; }
    [Required(ErrorMessage = "Status field is required.")]
    public byte Status { get; set; }
}

public class AddFeedbackRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Description field is required.")]
    public string Description { get; set; }
}

public class GetTotalAmountOfEmployerRequest
{
    [Required(ErrorMessage = "EmployeeId field is required.")]
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Start Date field is required.")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End Date field is required.")]
    public DateTime EndDate { get; set; }
}

public class AddInvoiceRequest
{
    [Required]
    public int EmployerId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required, MinLength(5)]
    public string Remarks { get; set; }
}

public class UpdateTimesheetByIdRequest
{
    [Required]
    public int TimesheetId { get; set; }
    [Required]
    public DateTime Date { get; set; } 
    [Required]
    public string StartTime { get; set; }
    [Required]
    public string EndTime { get; set; }
    [Required]
    public decimal HourlyRate { get; set; }
    [Required]
    public byte BreakTime { get; set; }
    public string? Notes { get; set; } = string.Empty;
}

public class UpdateEmployeeDetailsRequest
{
    [Required]
    public int EmployeeId { get; set; }
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    public byte EmployementTypeId { get; set; }
    [Required]
    public byte EmployeeTypeId { get; set; }
    [Required, MaxLength(255)]
    public string Address { get; set; }
    [MaxLength(255)]
    public string? Address2 { get; set; } = string.Empty;
    [Required, MaxLength(150)]
    public string City { get; set; }
    [Required, MaxLength(15)]
    public string PinCode { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required, MaxLength(250)]
    public string Nationality { get; set; }
    public string? UTRNumber { get; set; } = string.Empty;
    [Required]
    public double Latitude { get; set; }
    [Required]
    public double Longitude { get; set; }
    [MaxLength(15)]
    public string? DbsNumber { get; set; } = string.Empty;
    public string? NationalInsuranceNumber { get; set; } = string.Empty;
}

public class EmployeeLoginRequest
{
    [Required, StringLength(50), EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }
}
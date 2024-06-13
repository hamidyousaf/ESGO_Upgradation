namespace Application.DTOs.Requests;

public class EmployerRegisterRequest
{
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Email field is required."), MaxLength(62), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password field is required."), StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Confirm password field is required."), StringLength(50, MinimumLength = 5), Compare(nameof(Password), ErrorMessage = "Password mismatch")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Company name field is required."), MaxLength(255)]
    public string CompanyName { get; set; }
    [Required(ErrorMessage = "Company No field is required."), MaxLength(12)]
    public string CompanyNo { get; set; }

    [Required(ErrorMessage = "Company type field is required.")]
    public byte CompanyTypeId { get; set; } // Use CompanyTypeEnum

    [Required(ErrorMessage = "About organization field is required."), MaxLength(255)]
    public string AboutOrganization { get; set; }
    [Required(ErrorMessage = "Type of services field is required.")]
    public List<byte> TypeOfServices { get; set; } // Use TypeOfServiceEnum

    // site details
    [Required(ErrorMessage = "Site name field is required."), StringLength(50, MinimumLength = 5)]
    public string SiteName { get; set; }
    [Required(ErrorMessage = "PinCode field is required."), MaxLength(7)]
    public string PinCode { get; set; }
    [Required(ErrorMessage = "Nearest location field is required."), StringLength(255, MinimumLength = 5)]
    public string NearestLocation { get; set; }
    [Required(ErrorMessage = "Location field is required."), StringLength(255, MinimumLength = 5)]
    public string Location { get; set; }

    [Required(ErrorMessage = "Address field is required."), StringLength(255, MinimumLength = 5)]
    public string Address { get; set; }
    public string? Address2 { get; set; }

    // Contact details
    [Required(ErrorMessage ="Contact details are required.")]
    public  List<EmployerContactDetailRequest> ContactDetails { get; set; }
    [Required(ErrorMessage = "Health and safetyPolicy field is required.")]
    public bool IsHealthAndSafetyPolicy { get; set; }
}

public class EmployerLoginRequest
{
    [Required, StringLength(50), EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }
}
public class EmployerContactDetailRequest
{
    [Required(ErrorMessage = "Contact name field is required."), StringLength(100, MinimumLength = 5)]
    public string ContactName { get; set; }
    [Required(ErrorMessage = "Email field is required."), MaxLength(62), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Country code field is required."), MaxLength(5)]
    public string CountryCode { get; set; }
    [Required(ErrorMessage = "Contact number field is required."), MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Job title field is required."), MaxLength(255)]
    public string JobTitle { get; set; }
}

public class AddBookingRequest
{
    public int? EmployerId { get; set; }
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Date field is required.")]
    public DateTime Date { get; set; }
    [Required(ErrorMessage = "Details field is required."), MaxLength(1024)]
    public string Details { get; set; }
    public IFormFile? File { get; set; }

}

public class UpdateBookingRequest
{
    public int? EmployerId { get; set; }
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Date field is required.")]
    public DateTime Date { get; set; }
    [Required(ErrorMessage = "Details field is required."), MaxLength(1024)]
    public string Details { get; set; }
    public IFormFile? File { get; set; }

}

public class AddShiftRequest
{
    public int? EmployerId { get; set; }
    [Required(ErrorMessage = "Name field is required."), MaxLength(255)]
    public string Name { get; set; }
    [Required(ErrorMessage = "Start time field is required.")]
    public string StartTime { get; set; } // It should be this format => 12:00 
    [Required(ErrorMessage = "End time field is required.")]
    public string EndTime { get; set; } // It should be this format => 12:00 
}

public class UpdateShiftRequest
{
    [Required(ErrorMessage ="Id is required")]
    public int Id { get; set; }
    public int? EmployerId { get; set; }
    [Required(ErrorMessage = "Name field is required."), MaxLength(255)]
    public string Name { get; set; }
    [Required(ErrorMessage = "Start time field is required.")]
    public string StartTime { get; set; } // It should be this format => 12:00 
    [Required(ErrorMessage = "End time field is required.")]
    public string EndTime { get; set; } // It should be this format => 12:00 
}

public class DeleteShiftRequest
{
    [Required(ErrorMessage = "Shift id field is required")]
    public int ShiftId { get; set; }
    public int? EmployerId { get; set; }
}

public class DeleteBookingRequest
{
    [Required(ErrorMessage = "Booking id field is required")]
    public int BookingId { get; set; }
    public int? EmployerId { get; set; }
}

public class ChangeTimesheetStatusRequest
{
    public int EmployerId { get; set; } = 0;
    [Required(ErrorMessage = "Timesheet Id field is required")]
    public int TimesheetId { get; set; }
    [Required(ErrorMessage = "Status field is required")]
    public byte Status { get; set; }
    public byte Rating { get; set; } = 0;
    [Required(ErrorMessage = "ReviewedBy field is required")]
    public string ReviewedBy { get; set; }
    public string? Reason { get; set; } = string.Empty;
}

public class AddOrganisationImageRequest
{
    public int EmployerId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required")]
    public IFormFile File { get; set; }
}

public class AddOrganisationLogoRequest
{
    public int EmployerId { get; set; } = 0;
    [Required(ErrorMessage = "File field is required")]
    public IFormFile File { get; set; }
}

public class UpdateEmployerRequest
{
    public int EmployerId { get; set; } = 0;
    [Required(ErrorMessage = "Email field is required."), MaxLength(62), EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Company name field is required."), MaxLength(255)]
    public string CompanyName { get; set; }
    [Required(ErrorMessage = "PhoneNumber field is required."), MaxLength(11)]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Company type field is required.")]
    public byte CompanyTypeId { get; set; } // Use CompanyTypeEnum
    [Required(ErrorMessage = "PinCode field is required."), MaxLength(7)]
    public string PinCode { get; set; }
    [Required(ErrorMessage = "Site name field is required."), StringLength(50, MinimumLength = 5)]
    public string SiteName { get; set; }
    [Required(ErrorMessage = "Company No field is required."), MaxLength(12)]
    public string CompanyNo { get; set; }
    [Required(ErrorMessage = "JobTitle field is required."), MaxLength(255)]
    public string JobTitle { get; set; }
    [Required(ErrorMessage = "Location field is required."), StringLength(255, MinimumLength = 5)]
    public string Location { get; set; }

    [Required(ErrorMessage = "Address field is required."), StringLength(255, MinimumLength = 5)]
    public string Address { get; set; }
    public string? Address2 { get; set; }
    [Required(ErrorMessage = "About organization field is required."), MaxLength(255)]
    public string AboutOrganization { get; set; }
    [Required(ErrorMessage = "Type of services field is required.")]
    public List<byte> TypeOfServices { get; set; } // Use TypeOfServiceEnum
    // Contact details
    [Required(ErrorMessage = "Contact details are required.")]
    public List<EmployerContactDetailRequest> ContactDetails { get; set; }
}

public class UpdateJobRequest
{
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Job Id field is required.")]
    public int JobId { get; set; }
    [Required(ErrorMessage = "Employee Type Id field is required.")]
    public byte EmployeeTypeId { get; set; }
    [Required(ErrorMessage = "Employer Category Id field is required.")]
    public byte EmployeeCategoryId { get; set; }
    [Required(ErrorMessage = "Employee Id field is required.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Date field is required.")]
    public DateTime Date { get; set; }
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
    public bool IsFixedRate { get; set; }
    public decimal FixedRate { get; set; }
}

public class DeleteJobRequest
{
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Job Id field is required.")]
    public int JobId { get; set; }
    public string? Reason { get; set; } = string.Empty;
}

public class SelectEmployeeByAssignedJobIdRequest
{
    public int EmployerId { get; set; }
    [Required(ErrorMessage = "Assigned Job Id field is required.")]
    public int AssignedJobId { get; set; }
}

public class AddToFavouriteForEmployerRequest
{
    public int EmployerId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
}

public class UpdateFeedbackRequest
{
    [Required(ErrorMessage = "FeedbackId field is required.")]
    public int FeedbackId { get; set; }
    [Required(ErrorMessage = "Description field is required.")]
    public string Description { get; set; }
}
namespace Domain.DTOs.Requests;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    [Required]
    [StringLength(15)]
    public string Phone { get; set; }
    [Required]
    [StringLength(5)]
    public string PhoneRegion { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Title field is required.")]
    [MaxLength(5)]
    public string Title { get; set; }
    [Required(ErrorMessage = "Marital status field is required.")]
    [MaxLength(15)]
    public string MaritalStatus { get; set; }
    [MaxLength(7)]
    [Required(ErrorMessage = "Postal code field is required.")]
    public string PinCode { get; set; }
    //[RegularExpression("^[0-9]+$", ErrorMessage = "The field must contain only digits.")]
    [Required]
    public byte EmployeeTypeId { get; set; }
    //[RegularExpression("^[0-9]+$", ErrorMessage = "The field must contain only digits.")]
    [Required]
    public byte EmployementTypeId { get; set; }
    public IFormFile? File { get; set; }
    [Required(ErrorMessage = "Latitude field is required.")]
    public double Latitude { get; set; }
    [Required(ErrorMessage = "Longitude field is required.")]
    public double Longitude { get; set; }
    public string? Country { get; set; }
}

public class LoginRequest
{
    [Required, StringLength(50), EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }
}

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string NewPassword { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string ConfirmPassword { get; set; }
}
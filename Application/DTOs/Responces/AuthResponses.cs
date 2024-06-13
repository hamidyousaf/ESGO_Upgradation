namespace Domain.DTOs.Responces;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public string Country { get; set; }
    public byte EmployementTypeId { get; set; }
    public byte EmployeeTypeId { get; set; }
    public byte AccountStatus { get; set; }
}

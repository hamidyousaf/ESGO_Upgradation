namespace Application.DTOs.Requests;

public class GetEmployeeListByPaymentStatusRequest
{
    [Required]
    public DateTime Date1 { get; set; }
    [Required]
    public DateTime Date2 { get; set; }
    [Required]
    public byte Status { get; set; }
    [Required, DisplayName("Employee Type")]
    public byte EmployeeType { get; set; }
}

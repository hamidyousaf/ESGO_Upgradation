namespace Domain.Entities;

public class EmployerContactDetail : BaseEntity<int>
{
    public string ContactName { get; set; }
    public string Email { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public string JobTitle { get; set; }
    public int EmployerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property.
    public Employer Employer { get; set; }
}

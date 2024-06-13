namespace Domain.Entities;

public class Employer : BaseEntity<int>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string CompanyNo { get; set; }
    public byte CompanyTypeId { get; set; } // Use CompanyTypeEnum
    public string AboutOrganization { get; set; }
    public string OrganizationImageUrl { get; set; }
    public string OrganizationLogoUrl { get; set; }
    public string SiteName { get; set; }
    public string PinCode { get; set; }
    public string NearestLocation { get; set; }
    public string Location { get; set; }
    public string Address { get; set; }
    public string? Address2 { get; set; }
    public bool IsHealthAndSafetyPolicy { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public byte AccountStatus { get; set; }
    public decimal SelfCommission { get; set; }
    public decimal PayrollCommission { get; set; }
    public decimal LimitedCommission { get; set; }
    // navigational property.
    public ICollection<EmployerContactDetail> EmployerContactDetails { get; set; }
    public ICollection<TypeOfService> TypeOfServices { get; set; }
}

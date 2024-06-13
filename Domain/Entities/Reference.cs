namespace Domain.Entities;

public class Reference : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    // for personal.
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Relationship { get; set; }
    public string CharacterProfile { get; set; }

    // for professional.
    public string OrganizationName { get; set; }
    public string OrganizationEmail { get; set; }
    public string OrganizationPhoneNumber { get; set; }
    public string JobTitle { get; set; }
    public string Position { get; set; }
    public string BothWork { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool TillDate { get; set; }
    public string CharacterDescription { get; set; }
    //commomn
    public byte ReferenceTypeId { get; set; }
    public byte Status { get; set; }
    public string RejectionReason { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employee Employee { get; set; }
}
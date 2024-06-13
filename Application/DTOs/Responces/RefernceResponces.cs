namespace Application.DTOs.Responces;

public class GetPersonalReferenceByEmployeeIdResponce
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Relationship { get; set; }
    public string CharacterProfile { get; set; }
    public byte ReferenceTypeId { get; set; }
    public string ReferenceType { get => nameof(RT); }
    public byte Status { get; set; }
    public string RejectionReason { get; set; }
    [NotMapped]
    public ReferenceTypeEnum RT
    {
        get => (ReferenceTypeEnum)ReferenceTypeId;
        set => ReferenceTypeId = (byte)value;
    }
}
public class GetProfessionalReferenceByEmployeeIdResponce
{
    public string OrganizationName { get; set; }
    public string OrganizationEmail { get; set; }
    public string OrganizationPhoneNumber { get; set; }
    public string JobTitle { get; set; }
    public string Position { get; set; }
    public string BothWork { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CharacterDescription { get; set; }
    public byte ReferenceTypeId { get; set; }
    public string ReferenceType { get => nameof(RT); }
    public byte Status { get; set; }
    public bool TillDate { get; set; }
    public string RejectionReason { get; set; }
    [NotMapped]
    public ReferenceTypeEnum RT
    {
        get => (ReferenceTypeEnum)ReferenceTypeId;
        set => ReferenceTypeId = (byte)value;
    }
}

namespace Domain.Entities;

public  class Document : BaseEntity<byte>
{
    public string Name { get; set; }
    public byte EmployeeTypeId { get; set; }
    public byte Order { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    // navigational property
    public EmployeeType EmployeeType { get; set; }
}

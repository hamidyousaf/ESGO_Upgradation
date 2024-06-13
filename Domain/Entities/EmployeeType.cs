namespace Domain.Entities;

public sealed class EmployeeType : BaseEntity<byte>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MinRate { get; set; }
    public byte ParentId { get; set; }
    public byte Order { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}

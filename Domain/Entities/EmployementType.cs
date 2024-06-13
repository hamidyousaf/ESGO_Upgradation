namespace Domain.Entities;

public sealed class EmployementType : BaseEntity<byte>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte Order { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}

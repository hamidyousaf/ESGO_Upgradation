namespace Domain.Entities;

public class TypeOfService : BaseEntity<int>
{
    public byte TypeOfServiceId { get; set; }
    public int EmployerId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employer Employer { get; set; }
}

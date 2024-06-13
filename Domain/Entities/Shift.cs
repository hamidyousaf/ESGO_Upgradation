namespace Domain.Entities;

public class Shift : BaseEntity<int>
{
    public int EmployerId { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    // navigation property.
    public Employer Employer { get; set; }
}

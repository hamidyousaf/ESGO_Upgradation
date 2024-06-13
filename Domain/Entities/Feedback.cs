namespace Domain.Entities;

public class Feedback : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted{ get; set; }
    // navigational property.
    public Employee Employee { get; set; }
}

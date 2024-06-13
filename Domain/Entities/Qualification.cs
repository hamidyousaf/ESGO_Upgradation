namespace Domain.Entities;

public class Qualification : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public string Course { get; set; }
    public DateTime DateOfAward { get; set; }
    public string AwardingBody { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employee Employee { get; set; }
}

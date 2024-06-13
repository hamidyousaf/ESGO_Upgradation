namespace Domain.Entities;

public class Employement : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public string CompanyName { get; set;}
    public string CompanyAddress { get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Position { get; set; }
    public string ReasonForLeaving { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employee Employee { get; set; }
}

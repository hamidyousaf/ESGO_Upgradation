namespace Domain.Entities;

public class MonthlySupervisionReport : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public string Url { get; set; }
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employee Employee { get; set; }
}

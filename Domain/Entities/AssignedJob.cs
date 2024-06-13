namespace Domain.Entities;

public class AssignedJob : BaseEntity<int>
{
    public int JobId { get; set; }
    public int EmployeeId { get; set; }
    public byte JobStatus { get; set; }
    public int EmployerId { get; set; }
    public decimal HourWorked { get; set; }
    public DateTime AppliedDate { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public DateTime SelectedDate { get; set; }
    public bool IsSelected { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property.
    public Job Job { get; set; }  
    public Employee Employee { get; set; }  
    public Employer Employer { get; set; }  
}

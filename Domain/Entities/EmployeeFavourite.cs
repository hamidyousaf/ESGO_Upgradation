namespace Domain.Entities;

public class EmployeeFavourite
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int EmployerId { get; set; }
    // navigational property.
    public Employee Employee { get; set; }
    public Employer Employer { get; set; }
}

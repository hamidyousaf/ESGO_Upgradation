namespace Domain.Entities;

public class Invoice : BaseEntity<int>
{
    public int? EmployerId { get; set; }
    public DateTime Date { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal Amount { get; set; }
    public string Remarks { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employer? Employer { get; set; }
}

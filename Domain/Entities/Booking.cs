namespace Domain.Entities;

public class Booking : BaseEntity<int>
{
    public int EmployerId { get; set; }
    public DateTime Date { get; set; }
    public string Details { get; set; }
    public byte Status { get; set; }
    public string DocumentUrl { get; set; }
    public string ReasonForRejection { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property.
    public Employer Employer { get; set; }
}

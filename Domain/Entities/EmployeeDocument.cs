namespace Domain.Entities;

public class EmployeeDocument : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public byte DocumentId { get; set; }
    public string DocumentUrl { get; set; }
    public DateTime UploadedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public byte Status { get; set; }
    public string Reason { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property
    public Employee Employee { get; set; }
    public Document Document { get; set; }
}

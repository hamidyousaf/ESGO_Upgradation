namespace Domain.Entities;

public class DbsDocument : BaseEntity<int>
{
    public int EmployeeId { get; set; }
    public string Url { get; set; }
    public int DocumentTypeId { get; set; }
    public byte DocumentNumber { get; set; }
    public byte Status { get; set; }
    public string? RejectionReason { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    // navigational property.
    public Employee Employee { get; set; }
    public DocumentType DocumentType { get; set; }
}

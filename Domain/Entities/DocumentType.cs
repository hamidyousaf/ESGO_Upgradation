namespace Domain.Entities;

public class DocumentType : BaseEntity<int>
{
    public string Name { get; set; }
    public string GroupNo { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}

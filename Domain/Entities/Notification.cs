using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Notification : BaseEntity<int>
{
    public DateTime Date { get; set; }
    public byte Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public int? EmployeeId { get; set; }
    public int? EmployerId { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    [NotMapped] public string TimeAgo { get; set; }
}

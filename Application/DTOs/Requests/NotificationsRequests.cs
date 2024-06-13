namespace Domain.DTOs.Requests;

public class AddNotificationsRequest
{
    public int NotficationType { get; set; }
    public string NotificationContent { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string EmployeeUserId { get; set; } = string.Empty;
    public int JobId { get; set; }
    public int RoutingId { get; set; }
    public bool IsViewed { get; set; }
    public int PriorityType { get; set; }
    public string SiteName { get; set; } = string.Empty;
}
public class UpdateNotificationsRequest : AddNotificationsRequest
{
    public int Id { get; set; }
}


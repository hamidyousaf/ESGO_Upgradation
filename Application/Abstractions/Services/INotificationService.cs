namespace Application.Abstractions.Services;

public interface INotificationService
{
    Task<int> GetTotalUnreadNotification();
    Task TriggerNotification();
}

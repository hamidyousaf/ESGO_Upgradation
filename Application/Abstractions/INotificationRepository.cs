namespace Application.Abstractions;

public interface INotificationRepository : IGenericRepository<Notification>
{
    IQueryable<Notification> GetNotifications();
}

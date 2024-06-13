namespace Infrastructure.Repositories;

internal sealed class NotificationRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Notification>(dbContext), INotificationRepository
{
    public IQueryable<Notification> GetNotifications()
    {
        return GetAllReadOnly();
    }
}
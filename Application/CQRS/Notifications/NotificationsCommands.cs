namespace Domain.CQRS.Notifications;

public sealed class AddNotificationsCommand : IRequest<Result<int>>
{
    public AddNotificationsRequest Notifications { get; }
    public AddNotificationsCommand(AddNotificationsRequest notifications)
    {
        Notifications = notifications;
    }
}

public sealed class DeleteNotificationsCommand : IRequest<bool>
{
    public int NotificationsId { get; }
    public DeleteNotificationsCommand(int notificationsId)
    {
        NotificationsId = notificationsId;
    }
}
public sealed class UpdateNotificationsCommand : IRequest<bool>
{
    public UpdateNotificationsRequest Notifications { get; }
    public UpdateNotificationsCommand(UpdateNotificationsRequest notifications)
    {
        Notifications = notifications;
    }
}

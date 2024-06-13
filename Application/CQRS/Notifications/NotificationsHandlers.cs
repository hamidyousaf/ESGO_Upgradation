namespace Domain.CQRS.Notifications;
using Domain.Entities;
public sealed class AddNotificationsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddNotificationsCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddNotificationsCommand request, CancellationToken cancellationToken)
    {
        //var notifications = new Notification()
        //{
        //    NotficationType = request.Notifications.NotficationType,
        //    EmployeeUserId = request.Notifications.EmployeeUserId,
        //    SiteName = request.Notifications.SiteName,
        //    PriorityType = request.Notifications.PriorityType,
        //    IsViewed = request.Notifications.IsViewed,
        //    RoutingId = request.Notifications.RoutingId,
        //    JobId = request.Notifications.JobId,
        //    NotificationContent = request.Notifications.NotificationContent,
        //    UserId = request.Notifications.UserId,
        //};
        //await _unitOfWork.NotificationsRepository.Add(notifications);
        //await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(0, "Notifications added succesfully.");
    }
}

public sealed class GetAllNotificationsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAllNotificationsQuery, Result<GetNotificationsResponse>>
{
    public async Task<Result<GetNotificationsResponse>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.NotificationsRepository.GetAllReadOnly()
            .Select(n => new GetNotificationsResponse()
            {
                Id = n.Id,
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (notification is null)
        {
            return Result<GetNotificationsResponse>.Fail("No notifications found.");
        }

        return Result<GetNotificationsResponse>.Success(notification, "Notifications retrieved successfully.");
    }
}

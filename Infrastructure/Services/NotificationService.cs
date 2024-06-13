using Application.Abstractions.Services;
using Application.Helpers;
using Application.Helpers.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services;

public class NotificationService(IUnitOfWork _unitOfWork, IConfiguration _configuration, IHubContext<NotificationHub> _hubContext,
    UserManager<User> _userManger) : INotificationService
{

    public async Task<int> GetTotalUnreadNotification()
    {
        var total = await _unitOfWork.NotificationsRepository.GetAllReadOnly().Where(x => !x.IsRead).CountAsync();
        return total;
    }

    public async Task TriggerNotification()
    {
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin != null)
        {
            var connections = await PresenceTracker.GetConnectionsForUser(admin.UserName);
            var notifications = await GetTotalUnreadNotification();

            if (connections != null && notifications > 0)
            {
                await _hubContext.Clients.Clients(connections).SendAsync("GetTotalUnreadNotification", new { response = true, totalUnreadNotification = notifications });
            }
        }
    }
}
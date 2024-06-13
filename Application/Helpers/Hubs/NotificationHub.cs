using Application.Abstractions.Services;
using Microsoft.AspNetCore.SignalR;

namespace Application.Helpers.Hubs;

public class NotificationHub(IConfiguration _configuration, PresenceTracker _tracker, UserManager<User> _userManger, INotificationService _notificationService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin != null)
        {
            await _tracker.UserConnected(admin.UserName, Context.ConnectionId);
        }
    }
    public async Task TriggerNotification()
    {
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin != null)
        {
            var connections = await PresenceTracker.GetConnectionsForUser(admin.UserName);
            var notifications = await _notificationService.GetTotalUnreadNotification();

            if (connections != null && notifications > 0)
            {
                await Clients.Clients(connections).SendAsync("GetTotalUnreadNotification", new { response = true, totalUnreadNotification = notifications });
            }
        }
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var adminEmail = _configuration["SystemadminUser:Email"] ?? string.Empty;
        var admin = await _userManger.FindByEmailAsync(adminEmail);
        if (admin != null)
        {
            await _tracker.UserDisconnected(admin.UserName, Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
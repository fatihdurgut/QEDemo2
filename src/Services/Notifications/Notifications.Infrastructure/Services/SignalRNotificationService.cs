using Microsoft.AspNetCore.SignalR;
using Notifications.Application.DTOs;
using Notifications.Application.Services;

namespace Notifications.Infrastructure.Services;

/// <summary>
/// SignalR-based notification service implementation
/// </summary>
public class SignalRNotificationService<THub> : INotificationService where THub : Hub
{
    private readonly IHubContext<THub> _hubContext;

    public SignalRNotificationService(IHubContext<THub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendToUserAsync(Guid userId, string title, string message, string type)
    {
        var notification = new NotificationMessage(title, message, type);
        await _hubContext.Clients
            .Group($"user_{userId}")
            .SendAsync("ReceiveNotification", notification);
    }

    public async Task SendToRoleAsync(string role, string title, string message, string type)
    {
        var notification = new NotificationMessage(title, message, type);
        await _hubContext.Clients
            .Group($"role_{role}")
            .SendAsync("ReceiveNotification", notification);
    }

    public async Task BroadcastAsync(string title, string message, string type)
    {
        var notification = new NotificationMessage(title, message, type);
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
    }
}

namespace Notifications.Application.Services;

/// <summary>
/// Service for sending real-time notifications via SignalR
/// </summary>
public interface INotificationService
{
    Task SendToUserAsync(Guid userId, string title, string message, string type);
    Task SendToRoleAsync(string role, string title, string message, string type);
    Task BroadcastAsync(string title, string message, string type);
}

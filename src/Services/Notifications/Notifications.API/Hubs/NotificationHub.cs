using Microsoft.AspNetCore.SignalR;

namespace Notifications.API.Hubs;

/// <summary>
/// SignalR hub for real-time notifications
/// </summary>
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Join a user-specific notification group
    /// </summary>
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        _logger.LogInformation("Client {ConnectionId} joined user group: {UserId}", 
            Context.ConnectionId, userId);
    }

    /// <summary>
    /// Leave a user-specific notification group
    /// </summary>
    public async Task LeaveUserGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        _logger.LogInformation("Client {ConnectionId} left user group: {UserId}", 
            Context.ConnectionId, userId);
    }

    /// <summary>
    /// Join a role-based notification group
    /// </summary>
    public async Task JoinRoleGroup(string role)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"role_{role}");
        _logger.LogInformation("Client {ConnectionId} joined role group: {Role}", 
            Context.ConnectionId, role);
    }
}

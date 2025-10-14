using Microsoft.AspNetCore.Mvc;
using Notifications.Application.DTOs;
using Notifications.Application.Services;
using Notifications.Domain.Aggregates;
using Notifications.Domain.Repositories;
using Notifications.Domain.ValueObjects;

namespace Notifications.API.Controllers;

/// <summary>
/// Controller for managing notifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(
        INotificationRepository repository,
        INotificationService notificationService,
        ILogger<NotificationsController> logger)
    {
        _repository = repository;
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Get user notifications
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserNotifications(Guid userId)
    {
        var notifications = await _repository.GetByUserIdAsync(userId);
        var dtos = notifications.Select(n => new NotificationDto(
            n.Id, n.Title, n.Message, n.Type.ToString(), 
            n.UserId, n.Status.ToString(), n.CreatedAt, n.ReadAt));
        
        return Ok(dtos);
    }

    /// <summary>
    /// Get unread notifications for a user
    /// </summary>
    [HttpGet("user/{userId}/unread")]
    [ProducesResponseType(typeof(IEnumerable<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnreadNotifications(Guid userId)
    {
        var notifications = await _repository.GetUnreadByUserIdAsync(userId);
        var dtos = notifications.Select(n => new NotificationDto(
            n.Id, n.Title, n.Message, n.Type.ToString(), 
            n.UserId, n.Status.ToString(), n.CreatedAt, n.ReadAt));
        
        return Ok(dtos);
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    [HttpPost("{id}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var notification = await _repository.GetByIdAsync(id);
        if (notification == null)
            return NotFound();

        notification.MarkAsRead();
        await _repository.UpdateAsync(notification);
        
        return NoContent();
    }

    /// <summary>
    /// Send notification to a specific user
    /// </summary>
    [HttpPost("send/user/{userId}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> SendToUser(Guid userId, [FromBody] CreateNotificationRequest request)
    {
        try
        {
            if (!Enum.TryParse<NotificationType>(request.Type, out var type))
                return BadRequest(new { message = "Invalid notification type" });

            var notification = new Notification(request.Title, request.Message, type, userId);
            await _repository.AddAsync(notification);

            // Send real-time notification
            await _notificationService.SendToUserAsync(userId, request.Title, request.Message, request.Type);

            var dto = new NotificationDto(
                notification.Id, notification.Title, notification.Message,
                notification.Type.ToString(), notification.UserId,
                notification.Status.ToString(), notification.CreatedAt, notification.ReadAt);

            return CreatedAtAction(nameof(SendToUser), new { userId = userId }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while sending notification" });
        }
    }

    /// <summary>
    /// Send notification to all users with a specific role
    /// </summary>
    [HttpPost("send/role/{role}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SendToRole(string role, [FromBody] CreateNotificationRequest request)
    {
        try
        {
            // Send real-time notification to role group
            await _notificationService.SendToRoleAsync(role, request.Title, request.Message, request.Type);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to role {Role}", role);
            return StatusCode(500, new { message = "An error occurred while sending notification" });
        }
    }

    /// <summary>
    /// Broadcast notification to all users
    /// </summary>
    [HttpPost("send/broadcast")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Broadcast([FromBody] CreateNotificationRequest request)
    {
        try
        {
            // Broadcast to all connected clients
            await _notificationService.BroadcastAsync(request.Title, request.Message, request.Type);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting notification");
            return StatusCode(500, new { message = "An error occurred while broadcasting notification" });
        }
    }
}

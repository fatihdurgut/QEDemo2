namespace Notifications.Application.DTOs;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    string Type,
    Guid? UserId,
    string Status,
    DateTime CreatedAt,
    DateTime? ReadAt);

public record CreateNotificationRequest(
    string Title,
    string Message,
    string Type,
    Guid? UserId);

public record NotificationMessage(
    string Title,
    string Message,
    string Type);

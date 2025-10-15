using Notifications.Domain.ValueObjects;

namespace Notifications.Domain.Aggregates;

/// <summary>
/// Notification aggregate representing a notification message
/// </summary>
public class Notification
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public NotificationType Type { get; private set; }
    public Guid? UserId { get; private set; } // null for broadcast
    public NotificationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { } // For EF Core

    public Notification(string title, string message, NotificationType type, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty", nameof(message));

        Id = Guid.NewGuid();
        Title = title;
        Message = message;
        Type = type;
        UserId = userId;
        Status = NotificationStatus.Unread;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Unread)
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }

    public void MarkAsDeleted()
    {
        Status = NotificationStatus.Deleted;
    }
}

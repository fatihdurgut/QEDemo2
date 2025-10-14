namespace Notifications.Domain.ValueObjects;

/// <summary>
/// Types of notifications in the system
/// </summary>
public enum NotificationType
{
    Info = 1,
    Warning = 2,
    Error = 3,
    Success = 4,
    SaleCompleted = 5,
    InventoryLow = 6,
    NewOrder = 7
}

/// <summary>
/// Status of a notification
/// </summary>
public enum NotificationStatus
{
    Unread = 1,
    Read = 2,
    Deleted = 3
}

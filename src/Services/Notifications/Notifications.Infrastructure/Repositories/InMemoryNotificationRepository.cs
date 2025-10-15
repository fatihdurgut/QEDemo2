using Notifications.Domain.Aggregates;
using Notifications.Domain.Repositories;

namespace Notifications.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of notification repository for demonstration
/// TODO: Replace with EF Core implementation
/// </summary>
public class InMemoryNotificationRepository : INotificationRepository
{
    private readonly List<Notification> _notifications = new();

    public Task<Notification?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_notifications.FirstOrDefault(n => n.Id == id));
    }

    public Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
    {
        return Task.FromResult<IEnumerable<Notification>>(
            _notifications.Where(n => n.UserId == userId).ToList());
    }

    public Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId)
    {
        return Task.FromResult<IEnumerable<Notification>>(
            _notifications.Where(n => n.UserId == userId && 
                n.Status == Domain.ValueObjects.NotificationStatus.Unread).ToList());
    }

    public Task<Notification> AddAsync(Notification notification)
    {
        _notifications.Add(notification);
        return Task.FromResult(notification);
    }

    public Task UpdateAsync(Notification notification)
    {
        var existing = _notifications.FirstOrDefault(n => n.Id == notification.Id);
        if (existing != null)
        {
            _notifications.Remove(existing);
            _notifications.Add(notification);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == id);
        if (notification != null)
        {
            _notifications.Remove(notification);
        }
        return Task.CompletedTask;
    }
}

using Notifications.Domain.Aggregates;

namespace Notifications.Domain.Repositories;

/// <summary>
/// Repository interface for Notification aggregate
/// </summary>
public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid id);
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
    Task<Notification> AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Guid id);
}

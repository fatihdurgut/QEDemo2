using EventBus.Events;

namespace EventBus.Abstractions;

/// <summary>
/// Interface for event bus publish/subscribe operations
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event to all subscribers
    /// </summary>
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes to an integration event
    /// </summary>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Unsubscribes from an integration event
    /// </summary>
    void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}

using EventBus.Events;

namespace EventBus.Abstractions;

/// <summary>
/// Interface for integration event handlers
/// </summary>
public interface IIntegrationEventHandler<in TIntegrationEvent> 
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Handles the integration event
    /// </summary>
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}

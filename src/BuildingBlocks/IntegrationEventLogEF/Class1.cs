using EventBus.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace IntegrationEventLogEF;

/// <summary>
/// Entity for persisting integration events
/// </summary>
public class IntegrationEventLogEntry
{
    [Key]
    public Guid EventId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string EventTypeName { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreationTime { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string State { get; set; } = EventState.NotPublished.ToString();
    
    public int TimesSent { get; set; }
    
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.').Last();

    public IntegrationEventLogEntry()
    {
    }

    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        EventTypeName = @event.GetType().FullName ?? @event.GetType().Name;
        CreationTime = @event.CreatedDate;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = false
        });
        State = EventState.NotPublished.ToString();
        TimesSent = 0;
    }
}

public enum EventState
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFailed = 3
}


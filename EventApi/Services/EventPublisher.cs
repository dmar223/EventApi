using EventApi.Objects;

namespace EventApi.Services
{
    public class EventPublisher(
        IMessageQueueService messageQueueService, 
        ILogger<EventPublisher> logger) 
        : IPublisher<EventEnvelope>
    {
        public async ValueTask<PublishResult> PublishAsync(EventEnvelope eventEnvelope, CancellationToken cancellationToken)
        {
            try
            {
                return await messageQueueService.PublishAsync(eventEnvelope, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish event {EventId}", eventEnvelope.EventId);
                return PublishResult.Failed("Failed to publish event");
            }
        }
    }
}

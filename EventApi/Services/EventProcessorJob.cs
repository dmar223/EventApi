using EventApi.Objects;

namespace EventApi.Services
{
    internal sealed class EventProcessorJob(
    IMessageQueueService queue,
    ILogger<EventProcessorJob> logger)
    : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await foreach (EventEnvelope eventEnvelope in
                queue.ReadAllAsync(cancellationToken))
            {
                try
                {
                    logger.LogInformation(
                        "Processed event {EventId}: {Payload}",
                        eventEnvelope.EventId,
                        eventEnvelope.Payload?.ToString());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process event {EventId}", eventEnvelope.EventId);
                }
            }
        }
    }
}

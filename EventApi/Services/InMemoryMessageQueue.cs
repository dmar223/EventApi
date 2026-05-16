using EventApi.Objects;
using System.Threading.Channels;

namespace EventApi.Services
{
    public class InMemoryMessageQueue(
            Channel<EventEnvelope> channel,
            ILogger<InMemoryMessageQueue> logger) 
        : IMessageQueueService
    {
        public async ValueTask<PublishResult> PublishAsync(
            EventEnvelope eventEnvelope,
            CancellationToken cancellationToken)
        {
            try
            {
                await channel.Writer.WriteAsync(eventEnvelope, cancellationToken);
                return PublishResult.Successful();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish event {EventId}", eventEnvelope.EventId);
                return PublishResult.Failed("Failed to publish event");
            }
        }

        public IAsyncEnumerable<EventEnvelope> ReadAllAsync(CancellationToken cancellationToken)
        {
            return channel.Reader.ReadAllAsync(cancellationToken);
        }
    }
}

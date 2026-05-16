using EventApi.Objects;

namespace EventApi.Services
{
    public interface IMessageQueueService
    {
        public ValueTask<PublishResult> PublishAsync(
            EventEnvelope eventEnvelope,
            CancellationToken cancellationToken);

        public IAsyncEnumerable<EventEnvelope> ReadAllAsync(CancellationToken cancellationToken);
    }
}
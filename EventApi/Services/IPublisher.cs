using EventApi.Objects;

namespace EventApi.Services
{
    public interface IPublisher<T>
    {
        public ValueTask<PublishResult> PublishAsync(T eventEnvelope, CancellationToken cancellationToken);
    }
}
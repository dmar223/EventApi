using EventApi.Controllers;
using EventApi.Objects;
using EventApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;



namespace EventApiTests.Services
{
    [TestClass]
    public class EventPublisherTests
    {
        private IPublisher<EventEnvelope> _eventPublisher;
        Mock<ILogger<EventPublisher>> _loggerMock;
        Mock<IMessageQueueService> _messageQueueServiceMock;

        public EventPublisherTests()
        {
            _loggerMock = new Mock<ILogger<EventPublisher>>();
            _messageQueueServiceMock = new Mock<IMessageQueueService>();

            _eventPublisher = new EventPublisher(_messageQueueServiceMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task WhenPublishSucceeds_ThenReturnsSuccessResult()
        {
            EventEnvelope eventEnvelope = new EventEnvelope();
            _messageQueueServiceMock.Setup(x => x.PublishAsync(eventEnvelope, It.IsAny<CancellationToken>())).Returns(new ValueTask<PublishResult>(PublishResult.Successful()));

            var result = await _eventPublisher.PublishAsync(eventEnvelope, new CancellationToken());

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public async Task WhenPublishThrows_ThenReturnsFailedResult()
        {
            EventEnvelope eventEnvelope = new EventEnvelope() { EventId = "1ab" }; ;
            _messageQueueServiceMock.Setup(x => x.PublishAsync(eventEnvelope, It.IsAny<CancellationToken>())).Throws(new Exception("fail"));

            var result = await _eventPublisher.PublishAsync(eventEnvelope, new CancellationToken());

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task WhenPublishFails_ThenReturnsFailedResult()
        {
            EventEnvelope eventEnvelope = new EventEnvelope();
            _messageQueueServiceMock.Setup(x => x.PublishAsync(eventEnvelope, It.IsAny<CancellationToken>())).Returns(new ValueTask<PublishResult>(PublishResult.Failed("failed")));

            var result = await _eventPublisher.PublishAsync(eventEnvelope, new CancellationToken());

            Assert.IsFalse(result.Success);
        }
    }
}

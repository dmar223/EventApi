using EventApi.Controllers;
using EventApi.Objects;
using EventApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventApiTests.Controllers
{
    [TestClass]
    public sealed class EventsControllerTests
    {
        private EventsController _eventsController;
        Mock<ILogger<EventsController>> _loggerMock;
        Mock<IValidator<EventEnvelope>> _validatorMock;
        Mock<IPublisher<EventEnvelope>> _publisherMock;

        public EventsControllerTests()
        {
             _loggerMock = new Mock<ILogger<EventsController>>();
            _validatorMock = new Mock<IValidator<EventEnvelope>>();
            _publisherMock = new Mock<IPublisher<EventEnvelope>>();

            _eventsController = new EventsController(_validatorMock.Object, _publisherMock.Object, _loggerMock.Object);            
        }

        [TestMethod]
        public async Task WhenObjectIsValidAndPublishes_ThenReturnsAccepted()
        {
            EventEnvelope eventEnvelope = new EventEnvelope();
            _validatorMock.Setup(x => x.Validate(eventEnvelope)).Returns(new ValidationResult());
            _publisherMock.Setup(x => x.PublishAsync(eventEnvelope, It.IsAny<CancellationToken>())).Returns(new ValueTask<PublishResult>(PublishResult.Successful()));

            var result = await _eventsController.PostAsync(eventEnvelope, new CancellationToken());

            Assert.IsInstanceOfType<AcceptedResult>(result);
        }

        [TestMethod]
        public async Task WhenObjectIsInValid_ThenReturnsBadRequest()
        {
            EventEnvelope eventEnvelope = new EventEnvelope();
            ValidationResult validateResult = new ValidationResult();
            validateResult.AddError("error");

            _validatorMock.Setup(x => x.Validate(eventEnvelope)).Returns(validateResult);

            var result = await _eventsController.PostAsync(eventEnvelope, new CancellationToken());

            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }

        [TestMethod]
        public async Task WhenObjectIsInValidAndPublishFails_ThenReturnsServiceUnavailable()
        {
            EventEnvelope eventEnvelope = new EventEnvelope();
            PublishResult publishResult = new PublishResult() {  Success = false, Message = "failed" };

            _validatorMock.Setup(x => x.Validate(eventEnvelope)).Returns(new ValidationResult());
            _publisherMock.Setup(x => x.PublishAsync(eventEnvelope, It.IsAny<CancellationToken>())).Returns(new ValueTask<PublishResult>(publishResult));

            var result = await _eventsController.PostAsync(eventEnvelope, new CancellationToken());

            var objectResult = result as ObjectResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status503ServiceUnavailable, objectResult.StatusCode);
        }
    }
}

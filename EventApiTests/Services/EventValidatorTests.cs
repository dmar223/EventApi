using EventApi.Controllers;
using EventApi.Objects;
using EventApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventApiTests.Services
{
    [TestClass]
    public sealed class EventValidatorTests
    {
        private IValidator<EventEnvelope> _eventValidator;

        public EventValidatorTests()
        {
            _eventValidator = new EventValidator();
        }

        [TestMethod]
        public void WhenTimestampIsNotIsoCompliant_AddsValidationError()
        {
            EventEnvelope eventEnvelope = new EventEnvelope() { Timestamp = "9/10/2000" };

            var result = _eventValidator.Validate(eventEnvelope);

            Assert.IsFalse(result.IsValid);
            Assert.HasCount(1, result.ValidationErrors);
            Assert.AreEqual("timestamp not an ISO-8601 string", result.ValidationErrors.Single());
        }

        [TestMethod]
        public void WhenNoValidationIssues_ReturnsNoIssues()
        {
            EventEnvelope eventEnvelope = new EventEnvelope() { Timestamp = "2026-05-10T14:30:00Z" };

            var result = _eventValidator.Validate(eventEnvelope);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.ValidationErrors);
        }
    }
}

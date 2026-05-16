using EventApi.Objects;
using EventApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController(
        IValidator<EventEnvelope> eventValidator, 
        IPublisher<EventEnvelope> eventPublisher, 
        ILogger<EventsController> logger) 
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] EventEnvelope eventEnvelope, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = eventValidator.Validate(eventEnvelope);

            if (!validationResult.IsValid)
            {
                logger.LogWarning(
                "Event validation failed:{NewLine}{ValidationErrors}",
                Environment.NewLine,
                string.Join(Environment.NewLine, validationResult.ValidationErrors));

                return BadRequest(new
                {
                    errors = validationResult.ValidationErrors
                });
            }

            PublishResult publishResult = await eventPublisher.PublishAsync(eventEnvelope, cancellationToken);
            if (!publishResult.Success)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new { error = "Event queue unavailable" });
            }

            return Accepted();
        }
    }
}

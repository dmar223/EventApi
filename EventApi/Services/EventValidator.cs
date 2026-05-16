using EventApi.Objects;
using System.Globalization;

namespace EventApi.Services
{
    public class EventValidator : IValidator<EventEnvelope>
    {
        public ValidationResult Validate(EventEnvelope eventEnvelope)
        {
            var validationResult = new ValidationResult();
            if (eventEnvelope == null)
            {
                validationResult.AddError("eventEnvelope is null");
            }
            else
            {
                if (!IsValidTimestamp(eventEnvelope.Timestamp))
                {
                    validationResult.AddError($"timestamp not valid");
                }
            }

            return validationResult;
        }

        public static bool IsValidTimestamp(string? timestamp)
        {
            return DateTimeOffset.TryParseExact(
                timestamp,
                [
                "O",
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:sszzz"
                ],
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
        }
    }
}

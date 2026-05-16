using EventApi.Objects;

namespace EventApi.Services
{
    public interface IValidator<T>
    {
        public ValidationResult Validate(T eventEnvelope);
    }
}
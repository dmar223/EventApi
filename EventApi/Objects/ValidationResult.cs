namespace EventApi.Objects
{
    public class ValidationResult
    {
        private readonly List<string> _validationErrors = [];

        public bool IsValid => !_validationErrors.Any();

        public IReadOnlyCollection<string> ValidationErrors => _validationErrors;

        public void AddError(string error)
        {
            _validationErrors.Add(error);
        }
    }
}

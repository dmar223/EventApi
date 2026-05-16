namespace EventApi.Objects
{
    public class PublishResult
    {
        public bool Success { get; init; }

        public string? Message { get; set; }

        public static PublishResult Successful() =>
        new() { Success = true };

        public static PublishResult Failed(string message) =>
            new()
            {
                Success = false,
                Message = message
            };
    }
}

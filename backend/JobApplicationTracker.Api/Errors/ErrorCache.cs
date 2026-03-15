namespace JobApplicationTracker.Api.Errors
{
    public sealed record ErrorInfo(string ErrorCode, string ErrorMessage);

    public static class ErrorCache
    {
        public static readonly ErrorInfo RepositoryError =
            new("ERR_REPOSITORY", "An unexpected error occurred while accessing data. Please try again later.");

        public static readonly ErrorInfo NotFound =
            new("ERR_NOT_FOUND", "The requested resource was not found.");

        public static readonly ErrorInfo UnhandledError =
            new("ERR_UNHANDLED", "An unexpected error occurred. Please try again later.");

        // Add more centralized errors here as needed.
    }
}
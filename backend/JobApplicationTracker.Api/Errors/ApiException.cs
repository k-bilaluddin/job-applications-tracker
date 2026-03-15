namespace JobApplicationTracker.Api.Errors
{
    public class ApiException : Exception
    {
        public string ErrorCode { get; }
        public string ErrorMessage { get; }

        public ApiException(ErrorInfo errorInfo)
            : base(errorInfo.ErrorMessage)
        {
            ErrorCode = errorInfo.ErrorCode;
            ErrorMessage = errorInfo.ErrorMessage;
        }

        public ApiException(ErrorInfo errorInfo, Exception innerException)
            : base(errorInfo.ErrorMessage, innerException)
        {
            ErrorCode = errorInfo.ErrorCode;
            ErrorMessage = errorInfo.ErrorMessage;
        }
    }
}
namespace EmrCloudApi.Responses.Accounting
{
    public class CheckAccountingStatusResponse
    {
        public CheckAccountingStatusResponse(string errorType, string message)
        {
            ErrorType = errorType;
            Message = message;
        }

        public string ErrorType { get; private set; }
        public string Message { get; private set; }
    }
}

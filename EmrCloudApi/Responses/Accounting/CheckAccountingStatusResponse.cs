namespace EmrCloudApi.Responses.Accounting
{
    public class CheckAccountingStatusResponse
    {
        public CheckAccountingStatusResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}

namespace EmrCloudApi.Responses.Accounting
{
    public class SaveAccountingResponse
    {
        public SaveAccountingResponse(bool success)
        {
            Success = success;
        }
        public bool Success { get; private set; }
    }
}

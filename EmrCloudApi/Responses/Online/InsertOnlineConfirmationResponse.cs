namespace EmrCloudApi.Responses.Online
{
    public class InsertOnlineConfirmationResponse
    {
        public InsertOnlineConfirmationResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}

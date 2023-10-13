namespace EmrCloudApi.Responses.Lock
{
    public class UnlockResponse
    {
        public UnlockResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}

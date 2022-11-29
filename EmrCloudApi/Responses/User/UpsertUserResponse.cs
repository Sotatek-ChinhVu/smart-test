namespace EmrCloudApi.Responses.User
{
    public class UpsertUserResponse
    {
        public UpsertUserResponse(bool success)
        {
            Success = success;
        }
        public bool Success {get; private set; }
    }
}

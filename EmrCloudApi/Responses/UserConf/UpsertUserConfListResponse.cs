namespace EmrCloudApi.Responses.UserConf
{
    public class UpsertUserConfListResponse
    {
        public UpsertUserConfListResponse(List<UserConfItemValidationResponse> userConfItemValidationResponses)
        {
            UserConfItemValidationResponses = userConfItemValidationResponses;
        }

        public List<UserConfItemValidationResponse> UserConfItemValidationResponses { get; private set; }
    }

    public class UserConfItemValidationResponse
    {
        public UserConfItemValidationResponse(int position, string message)
        {
            Position = position;
            Message = message;
        }

        public int Position { get; private set; }

        public string Message { get; private set; }
    }
}

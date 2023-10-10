namespace EmrCloudApi.Responses.Auth
{
    public class AppTokenResponse
    {
        public string Token { get; private set; }

        public int UserId { get; private set; }

        public string LoginId { get; private set; }

        public AppTokenResponse(string token, int userId, string loginId)
        {
            Token = token;
            UserId = userId;
            LoginId = loginId;
        }
    }
}

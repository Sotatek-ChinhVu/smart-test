namespace SuperAdminAPI.Requests.UserToken
{
    public class RefreshTokenRequest
    {
        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }
    }
}

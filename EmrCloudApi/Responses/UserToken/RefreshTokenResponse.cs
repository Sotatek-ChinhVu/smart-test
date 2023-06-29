namespace EmrCloudApi.Responses.UserToken
{
    public class RefreshTokenResponse
    {
        public RefreshTokenResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }
    }
}

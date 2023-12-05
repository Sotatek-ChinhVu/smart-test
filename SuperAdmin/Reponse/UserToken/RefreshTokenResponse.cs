namespace SuperAdminAPI.Responses.UserToken
{
    public class RefreshTokenResponse
    {
        public RefreshTokenResponse(string accessToken, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime RefreshTokenExpiryTime {get; private set; }
    }
}

namespace EmrCloudApi.Responses.UserToken
{
    public class RefreshTokenResponse
    {
        public RefreshTokenResponse(string accessToken, string refreshToKen)
        {
            AccessToken = accessToken;
            RefreshToKen = refreshToKen;
        }

        public string AccessToken { get; private set; }

        public string RefreshToKen { get; private set; }
    }
}

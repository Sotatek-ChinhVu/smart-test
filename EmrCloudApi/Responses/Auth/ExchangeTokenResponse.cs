namespace EmrCloudApi.Responses.Auth;

public class ExchangeTokenResponse
{
    public ExchangeTokenResponse(string token, int userId)
    {
        Token = token;
        UserId = userId;
    }

    public string Token { get; private set; }
    public int UserId { get; private set; }
}

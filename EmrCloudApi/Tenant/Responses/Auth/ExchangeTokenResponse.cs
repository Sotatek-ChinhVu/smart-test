namespace EmrCloudApi.Tenant.Responses.Auth;

public class ExchangeTokenResponse
{
    public ExchangeTokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; private set; }
}

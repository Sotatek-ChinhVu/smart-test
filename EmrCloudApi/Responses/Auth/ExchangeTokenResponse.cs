namespace EmrCloudApi.Responses.Auth;

public class ExchangeTokenResponse
{
    public ExchangeTokenResponse(string token, int userId, string name, string kanaName)
    {
        Token = token;
        UserId = userId;
        Name = name;
        KanaName = kanaName;
    }

    public string Token { get; private set; }

    public int UserId { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }
}

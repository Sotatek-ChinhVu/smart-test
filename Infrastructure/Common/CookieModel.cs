using System.Text.Json.Serialization;

namespace Infrastructure.Common;

public class CookieModel
{
    [JsonConstructor]
    public CookieModel(string domain, string token)
    {
        Domain = domain;
        Token = token;
    }

    public CookieModel()
    {
        Domain = string.Empty;
        Token = string.Empty;
    }

    public string Domain { get; private set; }

    public string Token { get; private set; }
}

using System.Text.Json.Serialization;

namespace Infrastructure.Common;

public class CookieModel
{
    [JsonConstructor]
    public CookieModel(int hpId, int userId, string domain, string refreshToken)
    {
        HpId = hpId;
        UserId = userId;
        Domain = domain;
        RefreshToken = refreshToken;
    }

    public CookieModel()
    {
        HpId = -1;
        UserId = -1;
        Domain = string.Empty;
        RefreshToken = string.Empty;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string Domain { get; private set; }

    public string RefreshToken { get; private set; }
}

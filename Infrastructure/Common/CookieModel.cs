using System.Text.Json.Serialization;

namespace Infrastructure.Common;

public class CookieModel
{
    [JsonConstructor]
    public CookieModel(int hpId, string domain)
    {
        HpId = hpId;
        Domain = domain;
    }

    public CookieModel()
    {
        Domain = string.Empty;
    }

    public int HpId { get; private set; }

    public string Domain { get; private set; }
}

namespace EmrCloudApi.Responses.Auth;

public class ExchangeTokenResponse
{
    public ExchangeTokenResponse(string token, int userId, string loginId, string name, string kanaName, int kaId, bool isDoctor, int managerKbn, string sName, int hpId, string refreshToken, DateTime refreshTokenExpiryTime)
    {
        Token = token;
        UserId = userId;
        LoginId = loginId;
        Name = name;
        KanaName = kanaName;
        KaId = kaId;
        IsDoctor = isDoctor;
        ManagerKbn = managerKbn;
        SName = sName;
        HpId = hpId;
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }

    public string Token { get; private set; }

    public int UserId { get; private set; }

    public string LoginId { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int KaId { get; private set; }

    public bool IsDoctor { get; private set; }

    public int ManagerKbn { get; private set; }

    public string SName { get; private set; }

    public int HpId { get; private set; }

    public string RefreshToken { get; private set; }

    public DateTime RefreshTokenExpiryTime { get; private set; }
}

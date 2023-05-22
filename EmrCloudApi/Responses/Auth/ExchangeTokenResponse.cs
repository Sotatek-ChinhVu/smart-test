namespace EmrCloudApi.Responses.Auth;

public class ExchangeTokenResponse
{
    public ExchangeTokenResponse(string token, int userId, string name, string kanaName, int kaId, bool isDoctor, int managerKbn, string sName)
    {
        Token = token;
        UserId = userId;
        Name = name;
        KanaName = kanaName;
        KaId = kaId;
        IsDoctor = isDoctor;
        ManagerKbn = managerKbn;
        SName = sName;
    }

    public string Token { get; private set; }

    public int UserId { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int KaId { get; private set; }

    public bool IsDoctor { get; private set; }

    public int ManagerKbn { get; private set; }

    public string SName { get; private set; }
}

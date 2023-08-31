namespace EmrCloudApi.Requests.Online;

public class UpdateOQConfirmationRequest
{
    public long OnlineHistoryId { get; set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; set; } = new();

    public Dictionary<string, QuaConfirmationType> OnlQuaConfirmationTypeDict { get; set; } = new();
}

public class QuaConfirmationType
{
    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; } = string.Empty;
}

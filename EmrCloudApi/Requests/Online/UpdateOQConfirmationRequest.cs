namespace EmrCloudApi.Requests.Online;

public class UpdateOQConfirmationRequest
{
    public long OnlineHistoryId { get; set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; set; } = new();

    public Dictionary<string, OnlQuaConfirmationType> OnlQuaConfirmationTypeDict { get; set; } = new();
}

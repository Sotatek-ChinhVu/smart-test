namespace EmrCloudApi.Requests.Online;

public class SaveAllOQConfirmationRequest
{
    public long PtId { get; set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; set; } = new();

    public Dictionary<string, OnlQuaConfirmationType> OnlQuaConfirmationTypeDict { get; set; } = new();
}

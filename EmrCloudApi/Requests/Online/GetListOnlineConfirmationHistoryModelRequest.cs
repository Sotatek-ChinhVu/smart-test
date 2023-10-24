namespace EmrCloudApi.Requests.Online;

public class GetListOnlineConfirmationHistoryModelRequest
{
    public Dictionary<string, string> OnlQuaResFileDict { get; set; } = new();

    public Dictionary<string, OnlQuaConfirmationType> OnlQuaConfirmationTypeDict { get; set; } = new();
}

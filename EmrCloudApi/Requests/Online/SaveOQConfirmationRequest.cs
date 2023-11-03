namespace EmrCloudApi.Requests.Online;

public class SaveOQConfirmationRequest
{
    public long OnlineHistoryId { get; set; }

    public long PtId { get; set; }

    public string ConfirmationResult { get; set; } = string.Empty;

    public string OnlineConfirmationDate { get; set; } = string.Empty;

    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; } = string.Empty;

    public int UketukeStatus { get; set; }

    public bool IsUpdateRaiinInf { get; set; }
}

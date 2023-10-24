namespace EmrCloudApi.Requests.Online;

public class SaveOQConfirmationRequest
{
    public long OnlineHistoryId { get; set; }

    public long PtId { get; set; }

    public string ConfirmationResult { get; set; }

    public string OnlineConfirmationDate { get; set; }

    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; }

    public int UketukeStatus { get; set; }

    public bool IsUpdateRaiinInf { get; set; }
}

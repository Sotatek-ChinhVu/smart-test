namespace EmrCloudApi.Requests.Online;

public class UpdateOnlineInRaiinInfRequest
{
    public long PtId { get; set; }

    public string OnlineConfirmationDate { get; set; } = string.Empty;

    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; } = string.Empty;
}

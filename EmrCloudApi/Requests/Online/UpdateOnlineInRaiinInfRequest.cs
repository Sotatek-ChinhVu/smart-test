namespace EmrCloudApi.Requests.Online;

public class UpdateOnlineInRaiinInfRequest
{
    public long PtId { get; set; }

    public string OnlineConfirmationDate { get; set; }

    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; }
}

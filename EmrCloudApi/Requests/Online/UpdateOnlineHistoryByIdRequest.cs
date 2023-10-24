namespace EmrCloudApi.Requests.Online;

public class UpdateOnlineHistoryByIdRequest
{
    public long Id { get; set; }

    public long PtId { get; set; }

    public int UketukeStatus { get; set; }

    public int ConfirmationType { get; set; }
}

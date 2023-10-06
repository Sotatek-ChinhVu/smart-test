namespace EmrCloudApi.Requests.Online;

public class InsertOnlineConfirmHistoryRequestItem
{
    public long PtId { get; set; }

    public string ConfirmationResult { get; set; } = string.Empty;

    public int ConfirmationType { get; set; }

    public int UketukeStatus { get; set; }
}

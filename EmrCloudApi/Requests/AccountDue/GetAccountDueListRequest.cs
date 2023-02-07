namespace EmrCloudApi.Requests.AccountDue;

public class GetAccountDueListRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public bool IsUnpaidChecked { get; set; }
}

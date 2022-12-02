namespace EmrCloudApi.Requests.AccountDue;

public class SaveAccountDueListRequest
{
    public long PtId { get; set; }

    public int UserId { get; set; }

    public int SinDate { get; set; }

    public List<AccountDueItem> ListAccountDues { get; set; } = new();
}

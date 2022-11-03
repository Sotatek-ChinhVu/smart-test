namespace EmrCloudApi.Tenant.Requests.AccountDue;

public class GetAccountDueListRequest
{
    public int HpId { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public bool IsUnpaidChecked { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}

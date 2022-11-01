namespace EmrCloudApi.Tenant.Requests.AccountDue;

public class AccountDueItem
{
    public int NyukinKbn { get; set; }

    public long RaiinNo { get; set; }

    public int SortNo { get; set; }

    public int AdjustFutan { get; set; }

    public int NyukinGaku { get; set; }

    public int PaymentMethodCd { get; set; }

    public int NyukinDate { get; set; }

    public int UketukeSbt { get; set; }

    public string NyukinCmt { get; set; } = string.Empty;

    public int SeikyuGaku { get; set; }

    public int SeikyuTensu { get; set; }

    public string SeikyuDetail { get; set; } = string.Empty;

    public bool IsUpdated { get; set; }

    public long SeqNo { get; set; }

    public int RaiinInfStatus { get; set; }

    public int SeikyuAdjustFutan { get; set; }
}

namespace UseCase.AccountDue.SaveAccountDueList;

public class SyunoNyukinInputItem
{
    public SyunoNyukinInputItem(int nyukinKbn, long raiinNo, int sortNo, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int seikyuGaku, int seikyuTensu, string seikyuDetail, bool isUpdated, long seqNo, int raiinInfStatus)
    {
        NyukinKbn = nyukinKbn;
        RaiinNo = raiinNo;
        SortNo = sortNo;
        AdjustFutan = adjustFutan;
        NyukinGaku = nyukinGaku;
        PaymentMethodCd = paymentMethodCd;
        NyukinDate = nyukinDate;
        UketukeSbt = uketukeSbt;
        NyukinCmt = nyukinCmt;
        SeikyuGaku = seikyuGaku;
        SeikyuTensu = seikyuTensu;
        SeikyuDetail = seikyuDetail;
        IsUpdated = isUpdated;
        SeqNo = seqNo;
        RaiinInfStatus = raiinInfStatus;
    }

    public int NyukinKbn { get; private set; }

    public long RaiinNo { get; private set; }

    public int SortNo { get; private set; }

    public int AdjustFutan { get; private set; }

    public int NyukinGaku { get; private set; }

    public int PaymentMethodCd { get; private set; }

    public int NyukinDate { get; private set; }
    
    public int UketukeSbt { get; private set; }
    
    public string NyukinCmt { get; private set; }
    
    public int SeikyuGaku { get; private set; }

    public int SeikyuTensu { get; private set; }

    public string SeikyuDetail { get; private set; }

    public bool IsUpdated { get; private set; }

    public long SeqNo { get; private set; }

    public int RaiinInfStatus { get; private set; }
}

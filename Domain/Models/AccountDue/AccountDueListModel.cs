namespace Domain.Models.AccountDue;

public class AccountDueListModel
{
    public AccountDueListModel(int hpId, long ptId, int sinDate, int month, long raiinNo, long oyaRaiinNo, int nyukinKbn, int seikyuTensu, int seikyuGaku, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, int nyukinCmt, int unPaid, string stateDisplay, string sinDateDisplay, string kaDisplay, string hokenPatternName, string seikyuGakuDisplay, string newSeikyuGakuDisplay, string seikyuAdjustFutanDisplay, string newAdjustFutanDisplay, bool isMenjo, bool isNotPayment)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        Month = month;
        RaiinNo = raiinNo;
        OyaRaiinNo = oyaRaiinNo;
        NyukinKbn = nyukinKbn;
        SeikyuTensu = seikyuTensu;
        SeikyuGaku = seikyuGaku;
        AdjustFutan = adjustFutan;
        NyukinGaku = nyukinGaku;
        PaymentMethodCd = paymentMethodCd;
        NyukinDate = nyukinDate;
        UketukeSbt = uketukeSbt;
        NyukinCmt = nyukinCmt;
        UnPaid = unPaid;
        StateDisplay = stateDisplay;
        SinDateDisplay = sinDateDisplay;
        KaDisplay = kaDisplay;
        HokenPatternName = hokenPatternName;
        SeikyuGakuDisplay = seikyuGakuDisplay;
        NewSeikyuGakuDisplay = newSeikyuGakuDisplay;
        SeikyuAdjustFutanDisplay = seikyuAdjustFutanDisplay;
        NewAdjustFutanDisplay = newAdjustFutanDisplay;
        IsMenjo = isMenjo;
        IsNotPayment = isNotPayment;
    }


    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int Month { get; private set; }

    public long RaiinNo { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public int NyukinKbn { get; private set; }

    public int SeikyuTensu { get; private set; }

    public int SeikyuGaku { get; private set; }

    public int AdjustFutan { get; private set; }

    public int NyukinGaku { get; private set; }

    public int PaymentMethodCd { get; private set; }

    public int NyukinDate { get; private set; }

    public int UketukeSbt { get; private set; }

    public int NyukinCmt { get; private set; }

    public int UnPaid { get; private set; }


    // properties only display
    public string StateDisplay { get; private set; }

    public string SinDateDisplay { get; private set; }

    public string KaDisplay { get; private set; }

    public string HokenPatternName { get; private set; }

    public string SeikyuGakuDisplay { get; private set; }

    public string NewSeikyuGakuDisplay { get; private set; }

    public string SeikyuAdjustFutanDisplay { get; private set; }

    public string NewAdjustFutanDisplay { get; private set; }

    public bool IsMenjo { get; private set; }

    public bool IsNotPayment { get; private set; }
}

using Helper.Common;

namespace Domain.Models.AccountDue;

public class AccountDueModel
{
    public AccountDueModel(int hpId, long ptId, int seikyuSinDate, int month, long raiinNo, int hokenPid, long oyaRaiinNo, int nyukinKbn, int seikyuTensu, int seikyuGaku, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int unPaid, int newSeikyuGaku, int newAdjustFutan, string kaDisplay, string hokenPatternName, bool isSeikyuRow, int sortNo, long seqNo, string seikyuDetail, int seikyuAdjustFutan, bool isDelete)
    {
        HpId = hpId;
        PtId = ptId;
        SeikyuSinDate = seikyuSinDate;
        Month = month;
        RaiinNo = raiinNo;
        HokenPid = hokenPid;
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
        NewSeikyuGaku = newSeikyuGaku;
        NewAdjustFutan = newAdjustFutan;
        KaDisplay = kaDisplay;
        HokenPatternName = hokenPatternName;
        IsSeikyuRow = isSeikyuRow;
        SortNo = sortNo;
        SeqNo = seqNo;
        SeikyuDetail = seikyuDetail;
        SeikyuAdjustFutan = seikyuAdjustFutan;
        IsDelete = isDelete;
    }

    public AccountDueModel(int hpId, long ptId, int seikyuSinDate, int month, long raiinNo, int hokenPid, long oyaRaiinNo, int nyukinKbn, int seikyuTensu, int seikyuGaku, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int newSeikyuGaku, int newAdjustFutan, string kaDisplay, int sortNo, long seqNo, string seikyuDetail, int raiinInfStatus, int seikyuAdjustFutan)
    {
        HpId = hpId;
        PtId = ptId;
        SeikyuSinDate = seikyuSinDate;
        Month = month;
        RaiinNo = raiinNo;
        HokenPid = hokenPid;
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
        UnPaid = 0;
        NewSeikyuGaku = newSeikyuGaku;
        NewAdjustFutan = newAdjustFutan;
        KaDisplay = kaDisplay;
        HokenPatternName = string.Empty;
        IsSeikyuRow = true;
        SortNo = sortNo;
        SeqNo = seqNo;
        SeikyuDetail = seikyuDetail;
        RaiinInfStatus = raiinInfStatus;
        SeikyuAdjustFutan = seikyuAdjustFutan;
        IsDelete = false;
    }

    public AccountDueModel(int nyukinKbn, int sortNo, long raiinNo, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int seikyuGaku, int seikyuTensu, string seikyuDetail, long seqNo, int raiinInfStatus, int seikyuAdjustFutan, int seikyuSinDate, bool isDelete)
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
        SeqNo = seqNo;
        HpId = 0;
        PtId = 0;
        SeikyuSinDate = seikyuSinDate;
        Month = 0;
        HokenPid = 0;
        OyaRaiinNo = 0;
        UnPaid = 0;
        NewSeikyuGaku = 0;
        NewAdjustFutan = 0;
        KaDisplay = string.Empty;
        HokenPatternName = string.Empty;
        IsSeikyuRow = false;
        RaiinInfStatus = raiinInfStatus;
        SeikyuAdjustFutan = seikyuAdjustFutan;
        IsDelete = isDelete;
    }

    public AccountDueModel()
    {
        HpId = 0;
        PtId = 0;
        SeikyuSinDate = 0;
        Month = 0;
        RaiinNo = 0;
        HokenPid = 0;
        OyaRaiinNo = 0;
        NyukinKbn = 0;
        SeikyuTensu = 0;
        SeikyuGaku = 0;
        AdjustFutan = 0;
        NyukinGaku = 0;
        PaymentMethodCd = 0;
        NyukinDate = 0;
        UketukeSbt = 0;
        NyukinCmt = string.Empty;
        UnPaid = 0;
        NewSeikyuGaku = 0;
        NewAdjustFutan = 0;
        KaDisplay = string.Empty;
        HokenPatternName = string.Empty;
        IsSeikyuRow = false;
        SortNo = 0;
        SeqNo = 0;
        SeikyuDetail = string.Empty;
        SeikyuAdjustFutan = 0;
        IsDelete = false;
    }

    public AccountDueModel UpdateAccountDueListModel(int unPaid, string hokenPatternName, bool isSeikyuRow)
    {
        UnPaid = unPaid;
        HokenPatternName = hokenPatternName;
        IsSeikyuRow = isSeikyuRow;
        return this;
    }
    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SeikyuSinDate { get; private set; }

    public int Month { get; private set; }

    public long RaiinNo { get; private set; }

    public int HokenPid { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public int NyukinKbn { get; private set; }

    public int SeikyuTensu { get; private set; }

    public int SeikyuGaku { get; private set; }

    public int AdjustFutan { get; private set; }

    public int NyukinGaku { get; private set; }

    public int PaymentMethodCd { get; private set; }

    public int NyukinDate { get; private set; }

    public int UketukeSbt { get; private set; }

    public string NyukinCmt { get; private set; }

    public int UnPaid { get; private set; }

    public int NewSeikyuGaku { get; private set; }

    public int NewAdjustFutan { get; private set; }

    public string KaDisplay { get; private set; }

    public string HokenPatternName { get; private set; }

    public bool IsSeikyuRow { get; private set; }

    public int SortNo { get; private set; }

    public long SeqNo { get; private set; }

    public string SeikyuDetail { get; private set; }

    public int RaiinInfStatus { get; private set; }

    public int SeikyuAdjustFutan { get; private set; }

    public bool IsDelete { get; private set; }

}

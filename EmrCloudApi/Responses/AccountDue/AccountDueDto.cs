using Domain.Models.AccountDue;
using Helper.Common;

namespace EmrCloudApi.Responses.AccountDue;

public class AccountDueDto
{
    public AccountDueDto(AccountDueModel item)
    {
        HpId = item.HpId;
        PtId = item.PtId;
        SeikyuSinDate = item.SeikyuSinDate;
        Month = item.Month;
        RaiinNo = item.RaiinNo;
        HokenPid = item.HokenPid;
        OyaRaiinNo = item.OyaRaiinNo;
        NyukinKbn = item.NyukinKbn;
        SeikyuTensu = item.SeikyuTensu;
        SeikyuGaku = item.SeikyuGaku;
        AdjustFutan = item.AdjustFutan;
        NyukinGaku = item.NyukinGaku;
        PaymentMethodCd = item.PaymentMethodCd;
        NyukinDate = item.NyukinDate;
        UketukeSbt = item.UketukeSbt;
        NyukinCmt = item.NyukinCmt;
        UnPaid = item.UnPaid;
        NewSeikyuGaku = item.NewSeikyuGaku;
        NewAdjustFutan = item.NewAdjustFutan;
        KaDisplay = item.KaDisplay;
        HokenPatternName = item.HokenPatternName;
        IsSeikyuRow = item.IsSeikyuRow;
        SortNo = item.SortNo;
        SeqNo = item.SeqNo;
        SeikyuDetail = item.SeikyuDetail;
        RaiinInfStatus = item.RaiinInfStatus;
        SeikyuAdjustFutan = item.SeikyuAdjustFutan;
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

    // properties only display
    public string StateDisplay
    {
        get
        {
            switch (NyukinKbn)
            {
                case 1:
                    return "一部";
                case 2:
                    return "免除";
                case 3:
                    return "済";
                default:
                    return "未";
            }
        }
    }

    public string SinDateDisplay
    {
        get
        {
            return CIUtil.SDateToShowSDate(SeikyuSinDate);
        }
    }

    public string SeikyuGakuDisplay
    {
        get
        {
            return (SeikyuGaku + SeikyuAdjustFutan).ToString();
        }
    }

    public bool IsNewAdjustFutanDisplay
    {
        get => NewAdjustFutan != SeikyuAdjustFutan;
    }

    public string NewSeikyuGakuDisplay
    {
        get
        {
            if (IsNewAdjustFutanDisplay)
            {
                return "(" + (NewSeikyuGaku + NewAdjustFutan).ToString() + ")";
            }
            return "(" + (NewSeikyuGaku + SeikyuAdjustFutan).ToString() + ")";
        }
    }

    public string SeikyuAdjustFutanDisplay { get => SeikyuAdjustFutan.ToString(); }

    public string NewAdjustFutanDisplay
    {
        get
        {
            return "(" + NewAdjustFutan.ToString() + ")";
        }
    }

    public bool IsMenjo
    {
        get
        {
            if (NyukinKbn == 2)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsNotPayment
    {
        get
        {
            if (NyukinKbn == 0)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsShinSeikyuGaku
    {
        get => (NewSeikyuGaku != SeikyuGaku)
                || ((NewSeikyuGaku + NewAdjustFutan) != (SeikyuGaku + SeikyuAdjustFutan));
    }
}

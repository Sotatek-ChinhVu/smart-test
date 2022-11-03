﻿using Helper.Common;

namespace EmrCloudApi.Tenant.Responses.AccountDue;

public class AccountDueDto
{
    public AccountDueDto(int hpId, long ptId, int seikyuSinDate, int month, long raiinNo, int hokenPid, long oyaRaiinNo, int nyukinKbn, int seikyuTensu, int seikyuGaku, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int unPaid, int newSeikyuGaku, int newAdjustFutan, string kaDisplay, string hokenPatternName, bool isSeikyuRow, int sortNo, long seqNo, string seikyuDetail, int raiinInfStatus, int seikyuAdjustFutan)
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
        RaiinInfStatus = raiinInfStatus;
        SeikyuAdjustFutan = seikyuAdjustFutan;
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

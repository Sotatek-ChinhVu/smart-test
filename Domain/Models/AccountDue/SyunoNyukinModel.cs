using System.Text.Json.Serialization;

namespace Domain.Models.AccountDue;

public class SyunoNyukinModel
{
    [JsonConstructor]
    public SyunoNyukinModel(int hpId, long ptId, int sinDate, long raiinNo, long seqNo, int sortNo, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int nyukinjiTensu, int nyukinjiSeikyu, string nyukinjiDetail)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        SortNo = sortNo;
        AdjustFutan = adjustFutan;
        NyukinGaku = nyukinGaku;
        PaymentMethodCd = paymentMethodCd;
        NyukinDate = nyukinDate;
        UketukeSbt = uketukeSbt;
        NyukinCmt = nyukinCmt;
        NyukinjiTensu = nyukinjiTensu;
        NyukinjiSeikyu = nyukinjiSeikyu;
        NyukinjiDetail = nyukinjiDetail;
    }

    public SyunoNyukinModel()
    {
        NyukinCmt = string.Empty;
        NyukinjiDetail = string.Empty;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int AdjustFutan { get; private set; }

    public int NyukinGaku { get; private set; }

    public int PaymentMethodCd { get; private set; }

    public int NyukinDate { get; private set; }

    public int UketukeSbt { get; private set; }

    public string NyukinCmt { get; private set; }

    public int NyukinjiTensu { get; private set; }

    public int NyukinjiSeikyu { get; private set; }

    public string NyukinjiDetail { get; private set; }
}

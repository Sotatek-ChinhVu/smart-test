using Helper.Constants;

namespace Domain.Models.Receipt.Recalculation;

public class TodayOdrInfDetailModel
{
    public TodayOdrInfDetailModel(long ptId, int sinDate, long raiinNo, long rpNo, long rpEdaNo, int rowNo, int sinKouiKbn, string itemCd, string itemName, int drugKbn)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        RowNo = rowNo;
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        DrugKbn = drugKbn;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

    public int RowNo { get; private set; }

    public int SinKouiKbn { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int DrugKbn { get; private set; }

    public bool IsDrug
    {
        get => (SinKouiKbn == 20 && DrugKbn > 0) || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
            || (SinKouiKbn == 20 && ItemCd.StartsWith("Z"));
    }
}

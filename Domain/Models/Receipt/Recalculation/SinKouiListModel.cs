namespace Domain.Models.Receipt.Recalculation;

public class SinKouiListModel
{
    public SinKouiListModel(int createId, DateTime createDate, int hokenPid, int sinDate, long raiinNo, string itemCd, int sinKouiKbn, string tenItemCd, string name, int kohatuKbn, int yohoKbn, string ipnNameCd, int drugKbn, int isNodspKarte)
    {
        CreateId = createId;
        CreateDate = createDate;
        HokenPid = hokenPid;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        ItemCd = itemCd;
        SinKouiKbn = sinKouiKbn;
        TenItemCd = tenItemCd;
        Name = name;
        KohatuKbn = kohatuKbn;
        YohoKbn = yohoKbn;
        IpnNameCd = ipnNameCd;
        DrugKbn = drugKbn;
        IsNodspKarte = isNodspKarte;
    }

    public int CreateId { get; private set; }

    public DateTime CreateDate { get; private set; }

    public int HokenPid { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public string ItemCd { get; private set; }

    public int SinKouiKbn { get; private set; }

    public string TenItemCd { get; private set; }

    public string Name { get; private set; }

    public int KohatuKbn { get; private set; }

    public int YohoKbn { get; private set; }

    public string IpnNameCd { get; private set; }

    public int DrugKbn { get; private set; }

    public int IsNodspKarte { get; private set; }
}

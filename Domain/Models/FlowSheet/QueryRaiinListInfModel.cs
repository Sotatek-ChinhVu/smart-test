namespace Domain.Models.FlowSheet;

public class QueryRaiinListInfModel
{
    public QueryRaiinListInfModel(int sinDate, long raiinNo, int grpId, int kbnCd, int raiinListKbn, string kbnName, string colorCd)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        GrpId = grpId;
        KbnCd = kbnCd;
        RaiinListKbn = raiinListKbn;
        KbnName = kbnName;
        ColorCd = colorCd;
    }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int GrpId { get; private set; }

    public int KbnCd { get; private set; }

    public int RaiinListKbn { get; private set; }

    public string KbnName { get; private set; }

    public string ColorCd { get; private set; }
}

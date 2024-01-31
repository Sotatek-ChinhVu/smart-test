namespace Domain.Models.Yousiki;

public class RaiinListInfModel
{
    public RaiinListInfModel(long ptId, int sinDate, long raiinNo, int grpId, string grpName, int kbnCd, string kbnName, string colorCd, bool isContainsFile)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        GrpId = grpId;
        GrpName = grpName;
        KbnCd = kbnCd;
        ColorCd = colorCd;
        KbnName = kbnName;
        IsContainsFile = isContainsFile;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int GrpId { get; private set; }

    public string GrpName { get; private set; }

    public int KbnCd { get; private set; }

    public string KbnName { get; private set; }

    public string ColorCd { get; private set; }

    public bool IsContainsFile { get; private set; }
}

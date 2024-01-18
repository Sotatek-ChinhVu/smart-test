namespace Domain.Models.Yousiki;

public class RaiinListInfModel
{
    public RaiinListInfModel(long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, bool isContainsFile)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        GrpId = grpId;
        KbnCd = kbnCd;
        IsContainsFile = isContainsFile;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int GrpId { get; private set; }

    public int KbnCd { get; private set; }

    public bool IsContainsFile { get; private set; }
}

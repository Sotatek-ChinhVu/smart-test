namespace Domain.Models.Family;

public class RaiinInfModel
{
    public RaiinInfModel(long ptId, int sinDate, long raiinNo, int kaId, string kaName, int tantoId, string doctorName, int hokenPid, string hokenPatternName)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        KaId = kaId;
        KaName = kaName;
        TantoId = tantoId;
        DoctorName = doctorName;
        HokenPid = hokenPid;
        HokenPatternName = hokenPatternName;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int KaId { get; private set; }

    public string KaName { get; private set; }

    public int TantoId { get; private set; }

    public string DoctorName { get; private set; }

    public int HokenPid { get; private set; }

    public string HokenPatternName { get; private set; }
}

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
        TantoName = string.Empty;
        TantoKanaName = string.Empty;
    }

    public RaiinInfModel(long ptId, int sinDate, long raiinNo, int kaId, string kaName, int tantoId, string doctorName, string tantoName, string tantoKanaName, string kaSName)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        KaId = kaId;
        KaName = kaName;
        TantoId = tantoId;
        DoctorName = doctorName;
        TantoName = tantoName;
        TantoKanaName = tantoKanaName;
        KaSName = kaSName;
        HokenPatternName = string.Empty;
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

    public string TantoName { get; private set; }

    public string TantoKanaName { get; private set; }

    public string KaSName { get; private set; }
}

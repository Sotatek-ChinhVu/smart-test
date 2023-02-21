using UseCase.Family.GetRaiinInfList;

namespace EmrCloudApi.Responses.Family;

public class RaiinInfDto
{
    public RaiinInfDto(RaiinInfOutputItem output)
    {
        PtId = output.PtId;
        SinDate = output.SinDate;
        RaiinNo = output.RaiinNo;
        KaId = output.KaId;
        KaName = output.KaName;
        TantoId = output.TantoId;
        DoctorName = output.DoctorName;
        HokenPid = output.HokenPid;
        HokenPatternName = output.HokenPatternName;
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

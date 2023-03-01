using Domain.Models.Family;

namespace UseCase.Family.GetRaiinInfList;

public class RaiinInfOutputItem
{
    public RaiinInfOutputItem(RaiinInfModel model)
    {
        PtId = model.PtId;
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        KaId = model.KaId;
        KaName = model.KaName;
        TantoId = model.TantoId;
        DoctorName = model.DoctorName;
        HokenPid = model.HokenPid;
        HokenPatternName = model.HokenPatternName;
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

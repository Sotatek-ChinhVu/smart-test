using Domain.Models.Reception;
using Helper.Common;

namespace UseCase.Reception.GetRaiinListWithKanInf;

public class RaiinInfItem
{
    public RaiinInfItem(ReceptionModel receptionModel)
    {
        PtId = receptionModel.PtId;
        SinDate = receptionModel.SinDate;
        KaId = receptionModel.KaId;
        TantoId = receptionModel.TantoId;
        SName = receptionModel.SName;
        KaSname = receptionModel.KaSname;
        HokenKbnName = receptionModel.HokenKbnName;
    }

    public long PtId { get; private set; }
    
    public int SinDate { get; private set; }
    
    public int KaId { get; private set; }
    
    public int TantoId { get; private set; }
    
    public string SName { get; private set; }
    
    public string KaSname { get; private set; }
    
    public string HokenKbnName { get; private set; }

    public string SinDateLabel
    {
        get { return CIUtil.SDateToShowSDate(SinDate); }
    }
}

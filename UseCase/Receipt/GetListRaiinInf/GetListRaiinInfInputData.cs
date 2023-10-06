using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListRaiinInf;

public class GetListRaiinInfInputData : IInputData<GetListRaiinInfOutputData>
{
    public GetListRaiinInfInputData(int hpId, long ptId, int sinYm, int dayInMonth, int rpNo, int seqNo)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        DayInMonth = dayInMonth;
        RpNo = rpNo;
        SeqNo = seqNo;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DayInMonth { get; private set; }

    public int RpNo { get; private set; }

    public int SeqNo { get; private set; }
}

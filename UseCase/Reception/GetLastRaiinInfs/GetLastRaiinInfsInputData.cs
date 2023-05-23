using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetLastRaiinInfs;

public class GetLastRaiinInfsInputData : IInputData<GetLastRaiinInfsOutputData>
{
    public GetLastRaiinInfsInputData(int hpId, long ptId, int sinDate, bool isLastVisit)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        IsLastVisit = isLastVisit;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public bool IsLastVisit { get; private set; }
}

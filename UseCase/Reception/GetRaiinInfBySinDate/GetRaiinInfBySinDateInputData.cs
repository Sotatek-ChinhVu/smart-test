using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetRaiinInfBySinDate;

public class GetRaiinInfBySinDateInputData : IInputData<GetRaiinInfBySinDateOutputData>
{
    public GetRaiinInfBySinDateInputData(int hpId, int sinDate, long ptId)
    {
        HpId = hpId;
        SinDate = sinDate;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetVisitingInfs;

public class GetVisitingInfsInputData : IInputData<GetVisitingInfsOutputData>
{
    public GetVisitingInfsInputData(int hpId, long ptId, int sinYm)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetHpInf;

public class GetHpInfInputData : IInputData<GetHpInfOutputData>
{
    public GetHpInfInputData(int hpId, int sinDate)
    {
        HpId = hpId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }
}

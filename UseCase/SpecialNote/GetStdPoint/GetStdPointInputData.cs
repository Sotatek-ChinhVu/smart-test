using UseCase.Core.Sync.Core;

namespace UseCase.SpecialNote.GetStdPoint;

public class GetStdPointInputData : IInputData<GetStdPointOutputData>
{
    public GetStdPointInputData(int hpId, int sex)
    {
        HpId = hpId;
        Sex = sex;
    }

    public int HpId { get; private set; }

    public int Sex { get; private set; }
}

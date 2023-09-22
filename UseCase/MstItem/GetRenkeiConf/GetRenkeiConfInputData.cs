using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiConf;

public class GetRenkeiConfInputData : IInputData<GetRenkeiConfOutputData>
{
    public GetRenkeiConfInputData(int hpId, int renkeiSbt)
    {
        HpId = hpId;
        RenkeiSbt = renkeiSbt;
    }

    public int HpId { get; private set; }

    public int RenkeiSbt { get; private set; }
}

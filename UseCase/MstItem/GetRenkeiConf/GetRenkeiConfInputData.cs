using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiConf;

public class GetRenkeiConfInputData : IInputData<GetRenkeiConfOutputData>
{
    public GetRenkeiConfInputData(int hpId, int renkeiSbt, bool notLoadMst)
    {
        HpId = hpId;
        RenkeiSbt = renkeiSbt;
        NotLoadMst = notLoadMst;
    }

    public int HpId { get; private set; }

    public int RenkeiSbt { get; private set; }

    public bool NotLoadMst { get; private set; }
}

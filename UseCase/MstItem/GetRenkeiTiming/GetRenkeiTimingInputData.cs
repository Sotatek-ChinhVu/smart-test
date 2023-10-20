using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiTiming;

public class GetRenkeiTimingInputData : IInputData<GetRenkeiTimingOutputData>
{
    public GetRenkeiTimingInputData(int hpId, int renkeiId)
    {
        HpId = hpId;
        RenkeiId = renkeiId;
    }

    public int HpId { get; private set; }

    public int RenkeiId { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetNextUketukeNoBySetting;

public class GetNextUketukeNoBySettingOutputData : IOutputData
{
    public GetNextUketukeNoBySettingOutputData(GetNextUketukeNoBySettingStatus status, int nextUketukeNo)
    {
        Status = status;
        NextUketukeNo = nextUketukeNo;
    }

    public GetNextUketukeNoBySettingStatus Status { get; private set; }

    public int NextUketukeNo { get; private set; }
}

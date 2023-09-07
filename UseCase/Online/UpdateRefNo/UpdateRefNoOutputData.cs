using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateRefNo;

public class UpdateRefNoOutputData : IOutputData
{
    public UpdateRefNoOutputData(long nextRefNo, UpdateRefNoStatus status)
    {
        NextRefNo = nextRefNo;
        Status = status;
    }

    public long NextRefNo { get; private set; }

    public UpdateRefNoStatus Status { get; private set; }
}

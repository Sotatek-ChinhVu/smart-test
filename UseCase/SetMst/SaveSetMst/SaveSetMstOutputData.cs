using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.SaveSetMst;

public class SaveSetMstOutputData : IOutputData
{
    public SaveSetMstOutputData(SaveSetMstStatus status)
    {
        Status = status;
    }
    public SaveSetMstStatus Status { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceCheckCmtList;

public class SaveReceCheckCmtListOutputData : IOutputData
{
    public SaveReceCheckCmtListOutputData(SaveReceCheckCmtListStatus status)
    {
        Status = status;
    }

    public SaveReceCheckCmtListStatus Status { get; private set; }
}

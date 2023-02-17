using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListReceCmt;

public class SaveReceCmtListOutputData : IOutputData
{
    public SaveReceCmtListOutputData(SaveReceCmtListStatus status)
    {
        Status = status;
    }

    public SaveReceCmtListStatus Status { get; private set; }
}

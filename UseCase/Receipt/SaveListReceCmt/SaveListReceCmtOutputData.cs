using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListReceCmt;

public class SaveListReceCmtOutputData : IOutputData
{
    public SaveListReceCmtOutputData(SaveListReceCmtStatus status)
    {
        Status = status;
    }

    public SaveListReceCmtStatus Status { get; private set; }
}

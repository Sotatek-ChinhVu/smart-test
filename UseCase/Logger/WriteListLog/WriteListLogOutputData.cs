using UseCase.Core.Sync.Core;

namespace UseCase.Logger.WriteListLog;

public class WriteListLogOutputData : IOutputData
{
    public WriteListLogOutputData(WriteListLogStatus status)
    {
        Status = status;
    }

    public WriteListLogStatus Status { get; private set; }
}

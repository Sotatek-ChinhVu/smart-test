using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Insert;

public class InsertReceptionOutputData : IOutputData
{
    public InsertReceptionOutputData(InsertReceptionStatus status)
    {
        Status = status;
    }

    public InsertReceptionStatus Status { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Insert;

public class InsertReceptionOutputData : IOutputData
{
    public InsertReceptionOutputData(InsertReceptionStatus status, long raiinNo)
    {
        Status = status;
        RaiinNo = raiinNo;
    }

    public InsertReceptionStatus Status { get; private set; }
    public long RaiinNo { get; private set; }
}

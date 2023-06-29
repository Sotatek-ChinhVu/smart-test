using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Remove;

public class RemoveLockOutputData : IOutputData
{
    public RemoveLockOutputData(RemoveLockStatus status, List<ResponseLockModel> responseLockList)
    {
        Status = status;
        ResponseLockList = responseLockList;
    }

    public RemoveLockStatus Status { get; private set; }

    public List<ResponseLockModel> ResponseLockList { get; private set; }
}

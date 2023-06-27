using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Add;

public class AddLockOutputData : IOutputData
{
    public List<ResponseLockModel> ResponseLockList { get; private set; }

    public AddLockStatus Status { get; private set; }

    public AddLockOutputData(AddLockStatus status, List<ResponseLockModel> responseLockList)
    {
        Status = status;
        ResponseLockList = responseLockList;
    }
}

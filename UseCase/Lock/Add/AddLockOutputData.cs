using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Add;

public class AddLockOutputData : IOutputData
{
    public LockModel LockInf { get; private set; }

    public List<ResponseLockModel> ResponseLockModel { get; private set; } 

    public AddLockInputData AddLockInputData { get; private set; }

    public AddLockStatus Status { get; private set; }

    public AddLockOutputData(AddLockStatus status, LockModel lockInf, List<ResponseLockModel> responseLockModel)
    {
        Status = status;
        LockInf = lockInf;
        ResponseLockModel = responseLockModel;
        AddLockInputData = new();
    }

    public AddLockOutputData(AddLockStatus status, LockModel lockInf, List<ResponseLockModel> responseLockModel, AddLockInputData addLockInputData)
    {
        Status = status;
        LockInf = lockInf;
        ResponseLockModel = responseLockModel;
        AddLockInputData = addLockInputData;
    }
}
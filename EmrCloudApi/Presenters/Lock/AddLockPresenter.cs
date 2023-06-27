using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Add;

namespace EmrCloudApi.Presenters.Lock;

public class AddLockPresenter : IAddLockOutputPort
{
    public Response<LockResponse> Result { get; private set; } = new();

    public void Complete(AddLockOutputData outputData)
    {
        var lockInf = outputData.LockInf;
        Result.Data = new LockResponse(lockInf.UserId, lockInf.UserName, lockInf.LockLevel, lockInf.FunctionName);
        Result.Message = outputData.Status == AddLockStatus.Successed ? "Successed" : "The lock is existed!!!";
        Result.Status = (int)outputData.Status;
    }
}

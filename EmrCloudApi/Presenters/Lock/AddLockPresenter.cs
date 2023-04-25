using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Add;

namespace EmrCloudApi.Presenters.Lock;

public class AddLockPresenter : IAddLockOutputPort
{
    public Response<LockResponse> Result { get; private set; } = new();

    public void Complete(AddLockOutputData outputData)
    {
        Result.Data = new LockResponse(outputData.LockInf.UserName, outputData.LockInf.LockLevel, outputData.LockInf.FunctionName);
        Result.Message = outputData.Status == AddLockStatus.Successed ? "Successed" : "The lock is existed!!!";
        Result.Status = (int)outputData.Status;
    }
}

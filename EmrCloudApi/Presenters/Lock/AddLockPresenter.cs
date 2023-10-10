using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Add;
using UseCase.Lock.Get;

namespace EmrCloudApi.Presenters.Lock;

public class AddLockPresenter : IAddLockOutputPort
{
    public Response<LockResponse> Result { get; private set; } = new();

    public void Complete(AddLockOutputData outputData)
    {
        var lockInf = outputData.LockInf;
        Result.Data = new LockResponse(lockInf.UserId, lockInf.UserName, lockInf.LockLevel, lockInf.FunctionName, outputData.AddLockInputData);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(object status) => status switch
    {
        AddLockStatus.Successed => "Successed",
        AddLockStatus.Existed => "The lock is existed!!!",
        AddLockStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}

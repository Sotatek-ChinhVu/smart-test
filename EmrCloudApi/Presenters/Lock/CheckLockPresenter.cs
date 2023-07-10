using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Add;
using UseCase.Lock.Check;

namespace EmrCloudApi.Presenters.Lock;

public class CheckLockPresenter : ICheckLockOutputPort
{
    public Response<LockResponse> Result { get; private set; } = new();

    public void Complete(CheckLockOutputData outputData)
    {
        Result.Data = new LockResponse(outputData.LockInf.UserId, outputData.LockInf.UserName, outputData.LockInf.LockLevel, outputData.LockInf.FunctionName);
        Result.Message = outputData.Status == CheckLockStatus.Locked ? "Locked" : "Not lock";
        Result.Status = (int)outputData.Status;
    }
}

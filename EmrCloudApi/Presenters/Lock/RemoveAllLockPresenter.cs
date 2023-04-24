using EmrCloudApi.Responses;
using UseCase.Lock.Check;
using UseCase.Lock.Remove;

namespace EmrCloudApi.Presenters.Lock;

public class RemoveAllLockPresenter : IRemoveLockOutputPort
{
    public Response Result { get; private set; } = new();

    public void Complete(RemoveLockOutputData outputData)
    {
        Result.Message = outputData.Status == RemoveLockStatus.Successed ? "Successed" : "Failed";
        Result.Status = (int)outputData.Status;
    }
}

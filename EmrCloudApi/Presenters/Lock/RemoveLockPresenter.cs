using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Remove;

namespace EmrCloudApi.Presenters.Lock;

public class RemoveLockPresenter : IRemoveLockOutputPort
{
    public Response<UpdateVisitingLockResponse> Result { get; private set; } = new();

    public void Complete(RemoveLockOutputData outputData)
    {
        Result.Data = new UpdateVisitingLockResponse(outputData.ResponseLockList.Select(item => new ResponseLockDto(item)).ToList(), outputData.RemovedCount);
        Result.Message = outputData.Status == RemoveLockStatus.Successed ? "Successed" : "Failed";
        Result.Status = (int)outputData.Status;
    }
}

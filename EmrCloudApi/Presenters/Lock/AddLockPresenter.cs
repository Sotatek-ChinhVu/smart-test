using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.Add;

namespace EmrCloudApi.Presenters.Lock;

public class AddLockPresenter : IAddLockOutputPort
{
    public Response<UpdateVisitingLockResponse> Result { get; private set; } = new();

    public void Complete(AddLockOutputData outputData)
    {
        Result.Data = new UpdateVisitingLockResponse(outputData.ResponseLockList.Select(item => new ResponseLockDto(item)).ToList());
        Result.Message = outputData.Status == AddLockStatus.Successed ? "Successed" : "The lock is existed!!!";
        Result.Status = (int)outputData.Status;
    }
}

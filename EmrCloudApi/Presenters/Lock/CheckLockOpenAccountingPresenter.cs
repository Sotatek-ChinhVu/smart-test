using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Responses;
using UseCase.Lock.CheckLockOpenAccounting;

namespace EmrCloudApi.Presenters.Lock;

public class CheckLockOpenAccountingPresenter
{
    public Response<CheckLockOpenAccountingResponse> Result { get; private set; } = new();

    public void Complete(CheckLockOpenAccountingOutputData outputData)
    {
        Result.Data = new CheckLockOpenAccountingResponse(outputData.Status == CheckLockOpenAccountingStatus.Locked);
        Result.Message = outputData.Status == CheckLockOpenAccountingStatus.Locked ? "Locked" : "Not lock";
        Result.Status = (int)outputData.Status;
    }
}

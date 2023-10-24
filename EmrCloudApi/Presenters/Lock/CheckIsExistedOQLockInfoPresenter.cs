using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Responses;
using UseCase.Lock.CheckIsExistedOQLockInfo;

namespace EmrCloudApi.Presenters.Lock;

public class CheckIsExistedOQLockInfoPresenter : ICheckIsExistedOQLockInfoOutputPort
{
    public Response<CheckIsExistedOQLockInfoResponse> Result { get; private set; } = new();

    public void Complete(CheckIsExistedOQLockInfoOutputData outputData)
    {
        Result.Data = new CheckIsExistedOQLockInfoResponse(outputData.LockModel);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(object status) => status switch
    {
        CheckIsExistedOQLockInfoStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

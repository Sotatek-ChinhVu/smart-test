using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using UseCase.ReceSeikyu.CancelSeikyu;

namespace EmrCloudApi.Presenters.ReceSeikyu;

public class CancelSeikyuPresenter : ICancelSeikyuOutputPort
{
    public Response<CancelSeikyuResponse> Result { get; private set; } = new();

    public void Complete(CancelSeikyuOutputData output)
    {
        Result.Data = new CancelSeikyuResponse(output.Status == CancelSeikyuStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(CancelSeikyuStatus status) => status switch
    {
        CancelSeikyuStatus.Successed => ResponseMessage.Success,
        CancelSeikyuStatus.Failed => ResponseMessage.Failed,
        CancelSeikyuStatus.InvalidInputItem => ResponseMessage.InValid,
        _ => string.Empty
    };
}

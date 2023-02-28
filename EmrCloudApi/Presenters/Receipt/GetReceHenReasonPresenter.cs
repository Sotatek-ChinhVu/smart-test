using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceHenReason;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceHenReasonPresenter : IGetReceHenReasonOutputPort
{
    public Response<GetReceHenReasonResponse> Result { get; private set; } = new();

    public void Complete(GetReceHenReasonOutputData outputData)
    {
        Result.Data = new GetReceHenReasonResponse(outputData.ReceReasonCmt);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceHenReasonStatus status) => status switch
    {
        GetReceHenReasonStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

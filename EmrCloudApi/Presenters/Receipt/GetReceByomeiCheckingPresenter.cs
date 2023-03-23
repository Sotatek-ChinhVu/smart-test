using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceByomeiChecking;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceByomeiCheckingPresenter : IGetReceByomeiCheckingOutputPort
{
    public Response<GetReceByomeiCheckingResponse> Result { get; private set; } = new();

    public void Complete(GetReceByomeiCheckingOutputData outputData)
    {
        Result.Data = new GetReceByomeiCheckingResponse(outputData);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceByomeiCheckingStatus status) => status switch
    {
        GetReceByomeiCheckingStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

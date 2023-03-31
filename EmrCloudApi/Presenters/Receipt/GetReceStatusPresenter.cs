using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceStatus;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceStatusPresenter : IGetReceStatusOutputPort
{
    public Response<GetReceStatusResponse> Result { get; private set; } = new();

    public void Complete(GetReceStatusOutputData outputData)
    {
        Result.Data = new GetReceStatusResponse(outputData.ReceStatus);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceStatusStatus status) => status switch
    {
        GetReceStatusStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

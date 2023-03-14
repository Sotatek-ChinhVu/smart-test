using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.DoReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class DoReceCmtPresenter : IDoReceCmtOutputPort
{
    public Response<GetReceCmtListResponse> Result { get; private set; } = new();

    public void Complete(DoReceCmtOutputData outputData)
    {
        Result.Data = new GetReceCmtListResponse(outputData.ReceCmtList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(DoReceCmtStatus status) => status switch
    {
        DoReceCmtStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
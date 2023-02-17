using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceCmtListPresenter : IGetReceCmtListOutputPort
{
    public Response<GetReceCmtListResponse> Result { get; private set; } = new();

    public void Complete(GetReceCmtListOutputData outputData)
    {
        Result.Data = new GetReceCmtListResponse(outputData.ReceCmtList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceCmtListStatus status) => status switch
    {
        GetReceCmtListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
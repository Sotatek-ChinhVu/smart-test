using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetRecePreviewList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetRecePreviewListPresenter : IGetRecePreviewListOutputPort
{
    public Response<GetRecePreviewListResponse> Result { get; private set; } = new();

    public void Complete(GetRecePreviewListOutputData outputData)
    {
        Result.Data = new GetRecePreviewListResponse(outputData.ReceInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetRecePreviewListStatus status) => status switch
    {
        GetRecePreviewListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

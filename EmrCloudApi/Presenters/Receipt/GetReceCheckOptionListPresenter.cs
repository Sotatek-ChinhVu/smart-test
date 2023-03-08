using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceCheckOptionList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceCheckOptionListPresenter : IGetReceCheckOptionListOutputPort
{
    public Response<GetReceCheckOptionListResponse> Result { get; private set; } = new();

    public void Complete(GetReceCheckOptionListOutputData outputData)
    {
        Result.Data = new GetReceCheckOptionListResponse(outputData.ReceCheckOptionData);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceCheckOptionListStatus status) => status switch
    {
        GetReceCheckOptionListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceiCheckList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceiCheckListPresenter : IGetReceiCheckListOutputPort
{
    public Response<GetReceiCheckListResponse> Result { get; private set; } = new();

    public void Complete(GetReceiCheckListOutputData outputData)
    {
        Result.Data = new GetReceiCheckListResponse(outputData.ReceiptCheckCmtErrList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceiCheckListStatus status) => status switch
    {
        GetReceiCheckListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

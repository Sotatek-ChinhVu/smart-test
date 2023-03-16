using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.ReceiptEdit;

namespace EmrCloudApi.Presenters.Receipt;

public class GetReceiptEditPresenter : IGetReceiptEditOutputPort
{
    public Response<GetReceiptEditResponse> Result { get; private set; } = new();

    public void Complete(GetReceiptEditOutputData outputData)
    {
        Result.Data = new GetReceiptEditResponse(outputData.ReceiptEditOrigin, outputData.ReceiptEditCurrent, outputData.TokkiMstDictionary);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetReceiptEditStatus status) => status switch
    {
        GetReceiptEditStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
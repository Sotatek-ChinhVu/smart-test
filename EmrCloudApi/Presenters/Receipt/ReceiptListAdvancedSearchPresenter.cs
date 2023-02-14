using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.ReceiptListAdvancedSearch;

namespace EmrCloudApi.Presenters.Receipt;

public class ReceiptListAdvancedSearchPresenter : IReceiptListAdvancedSearchOutputPort
{
    public Response<ReceiptListAdvancedSearchResponse> Result { get; private set; } = new();

    public void Complete(ReceiptListAdvancedSearchOutputData outputData)
    {
        Result.Data = new ReceiptListAdvancedSearchResponse(outputData.ListReceipt);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(ReceiptListAdvancedSearchStatus status) => status switch
    {
        ReceiptListAdvancedSearchStatus.Successed => ResponseMessage.Success,
        ReceiptListAdvancedSearchStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}

using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.ReceCmtHistory;

namespace EmrCloudApi.Presenters.Receipt;

public class ReceCmtHistoryPresenter : IReceCmtHistoryOutputPort
{
    public Response<ReceCmtHistoryResponse> Result { get; private set; } = new();

    public void Complete(ReceCmtHistoryOutputData outputData)
    {
        Result.Data = new ReceCmtHistoryResponse(outputData.ReceCmtList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(ReceCmtHistoryStatus status) => status switch
    {
        ReceCmtHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

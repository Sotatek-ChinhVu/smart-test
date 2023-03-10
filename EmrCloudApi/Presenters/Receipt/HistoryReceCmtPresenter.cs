using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.HistoryReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class HistoryReceCmtPresenter : IHistoryReceCmtOutputPort
{
    public Response<HistoryReceCmtResponse> Result { get; private set; } = new();

    public void Complete(HistoryReceCmtOutputData outputData)
    {
        Result.Data = new HistoryReceCmtResponse(outputData.ReceCmtList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(HistoryReceCmtStatus status) => status switch
    {
        HistoryReceCmtStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

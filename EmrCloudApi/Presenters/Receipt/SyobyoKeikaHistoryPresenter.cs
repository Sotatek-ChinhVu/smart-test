using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SyobyoKeikaHistory;

namespace EmrCloudApi.Presenters.Receipt;

public class SyobyoKeikaHistoryPresenter : ISyobyoKeikaHistoryOutputPort
{
    public Response<SyobyoKeikaHistoryResponse> Result { get; private set; } = new();

    public void Complete(SyobyoKeikaHistoryOutputData outputData)
    {
        Result.Data = new SyobyoKeikaHistoryResponse(outputData.SyobyoKeikaList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SyobyoKeikaHistoryStatus status) => status switch
    {
        SyobyoKeikaHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

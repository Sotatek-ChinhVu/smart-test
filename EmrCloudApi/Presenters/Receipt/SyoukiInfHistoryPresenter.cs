using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SyoukiInfHistory;

namespace EmrCloudApi.Presenters.Receipt;

public class SyoukiInfHistoryPresenter : ISyoukiInfHistoryOutputPort
{
    public Response<SyoukiInfHistoryResponse> Result { get; private set; } = new();

    public void Complete(SyoukiInfHistoryOutputData outputData)
    {
        Result.Data = new SyoukiInfHistoryResponse(outputData.SyoukiInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SyoukiInfHistoryStatus status) => status switch
    {
        SyoukiInfHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}


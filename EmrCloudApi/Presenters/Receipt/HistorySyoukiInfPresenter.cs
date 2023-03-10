using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.HistorySyoukiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class HistorySyoukiInfPresenter : IHistorySyoukiInfOutputPort
{
    public Response<HistorySyoukiInfResponse> Result { get; private set; } = new();

    public void Complete(HistorySyoukiInfOutputData outputData)
    {
        Result.Data = new HistorySyoukiInfResponse(outputData.SyoukiInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(HistorySyoukiInfStatus status) => status switch
    {
        HistorySyoukiInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}


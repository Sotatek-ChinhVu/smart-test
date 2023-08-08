using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListRaiinInf;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListRaiinInfPresenter : IGetListRaiinInfOutputPort
{
    public Response<GetListRaiinInfResponse> Result { get; private set; } = new();

    public void Complete(GetListRaiinInfOutputData outputData)
    {
        Result.Data = new GetListRaiinInfResponse(outputData.RaiinInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListRaiinInfStatus status) => status switch
    {
        GetListRaiinInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

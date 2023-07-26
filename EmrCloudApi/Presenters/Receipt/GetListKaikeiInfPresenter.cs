using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListKaikeiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListKaikeiInfPresenter : IGetListKaikeiInfOutputPort
{
    public Response<GetListKaikeiInfResponse> Result { get; private set; } = new();

    public void Complete(GetListKaikeiInfOutputData outputData)
    {
        Result.Data = new GetListKaikeiInfResponse(outputData.PtHokenInfKaikeiList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListKaikeiInfStatus status) => status switch
    {
        GetListKaikeiInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

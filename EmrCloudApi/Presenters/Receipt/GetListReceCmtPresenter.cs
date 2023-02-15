using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListReceCmtPresenter : IGetListReceCmtOutputPort
{
    public Response<GetListReceCmtResponse> Result { get; private set; } = new();

    public void Complete(GetListReceCmtOutputData outputData)
    {
        Result.Data = new GetListReceCmtResponse(outputData.ReceCmtList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListReceCmtStatus status) => status switch
    {
        GetListReceCmtStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
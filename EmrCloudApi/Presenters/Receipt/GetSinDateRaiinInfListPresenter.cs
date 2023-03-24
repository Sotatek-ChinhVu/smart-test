using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetSinDateRaiinInfList;

namespace EmrCloudApi.Presenters.Receipt;

public class GetSinDateRaiinInfListPresenter : IGetSinDateRaiinInfListOutputPort
{
    public Response<GetSinDateRaiinInfListResponse> Result { get; private set; } = new();

    public void Complete(GetSinDateRaiinInfListOutputData outputData)
    {
        Result.Data = new GetSinDateRaiinInfListResponse(outputData.SinDateList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetSinDateRaiinInfListStatus status) => status switch
    {
        GetSinDateRaiinInfListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

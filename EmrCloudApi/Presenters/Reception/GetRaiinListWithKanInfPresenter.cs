using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetRaiinListWithKanInf;

namespace EmrCloudApi.Presenters.Reception;

public class GetRaiinListWithKanInfPresenter : IGetRaiinListWithKanInfOutputPort
{
    public Response<GetRaiinListWithKanInfResponse> Result { get; private set; } = new();

    public void Complete(GetRaiinListWithKanInfOutputData output)
    {
        Result.Data = new GetRaiinListWithKanInfResponse(output.RaiinInfList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRaiinListWithKanInfStatus status) => status switch
    {
        GetRaiinListWithKanInfStatus.Successed => ResponseMessage.Success,
        GetRaiinListWithKanInfStatus.InvalidPtId => ResponseMessage.NotFoundPtInf,
        _ => string.Empty
    };
}

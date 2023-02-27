using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetRaiinInfList;

namespace EmrCloudApi.Presenters.Family;

public class GetRaiinInfListPresenter : IGetListRaiinInfOutputPort
{
    public Response<GetRaiinInfListResponse> Result { get; private set; } = new();

    public void Complete(GetRaiinInfListOutputData output)
    {
        Result.Data = new GetRaiinInfListResponse(output.RaiinInfList.Select(item => new RaiinInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRaiinInfListStatus status) => status switch
    {
        GetRaiinInfListStatus.Successed => ResponseMessage.Success,
        GetRaiinInfListStatus.InvalidPtId => ResponseMessage.NotFoundPtInf,
        _ => string.Empty
    };
}

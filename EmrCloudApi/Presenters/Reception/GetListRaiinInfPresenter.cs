using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.GetListRaiinInfs;

namespace EmrCloudApi.Tenant.Presenters.Reception;
public class GetListRaiinInfPresenter : IGetListRaiinInfsOutputPort
{
    public Response<GetListRaiinInfResponse> Result { get; private set; } = new Response<GetListRaiinInfResponse>();

    public void Complete(GetListRaiinInfsOutputData output)
    {
        Result.Data = new GetListRaiinInfResponse(output.RaiinInfs);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListRaiinInfsStatus status) => status switch
    {
        GetListRaiinInfsStatus.Success => ResponseMessage.Success,
        GetListRaiinInfsStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetListRaiinInfsStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}

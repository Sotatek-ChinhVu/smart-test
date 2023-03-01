using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.GetListRaiinInfs;

namespace EmrCloudApi.Tenant.Presenters.Reception;
public class GetListRaiinInfPresenter : IGetListRaiinInfOutputPort
{
    public Response<GetListRaiinInfResponse> Result { get; private set; } = new Response<GetListRaiinInfResponse>();

    public void Complete(GetListRaiinInfOutputData output)
    {
        Result.Data = new GetListRaiinInfResponse(output.RaiinInfs);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListRaiinInfStatus status) => status switch
    {
        GetListRaiinInfStatus.Success => ResponseMessage.Success,
        GetListRaiinInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetListRaiinInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        GetListRaiinInfStatus.InvalidPageIndex => ResponseMessage.InvalidPageIndex,
        GetListRaiinInfStatus.InvalidPageSize => ResponseMessage.InvalidPageSize,
        _ => string.Empty
    };
}

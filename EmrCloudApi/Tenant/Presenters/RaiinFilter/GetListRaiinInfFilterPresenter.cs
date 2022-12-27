using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinFilter;
using UseCase.RaiinFilterMst.GetListRaiinInf;
using UseCase.Reception.GetLastRaiinInfs;

namespace EmrCloudApi.Tenant.Presenters.RaiinFilter;

public class GetListRaiinInfFilterPresenter : IGetListRaiinInfFilterOutputPort
{
    public Response<GetListRaiinInfFilterResponse> Result { get; private set; } = new Response<GetListRaiinInfFilterResponse>();

    public void Complete(GetListRaiinInfFilterOutputData output)
    {
        Result.Data = new GetListRaiinInfFilterResponse(output.RaiinInfFilters);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListRaiinInfFilterStatus status) => status switch
    {
        GetListRaiinInfFilterStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}

using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinFilter;
using UseCase.RaiinFilterMst.GetList;

namespace EmrCloudApi.Tenant.Presenters.RaiinFilter;

public class GetRaiinFilterMstListPresenter : IGetRaiinFilterMstListOutputPort
{
    public Response<GetRaiinFilterMstListResponse> Result { get; private set; } = new Response<GetRaiinFilterMstListResponse>();

    public void Complete(GetRaiinFilterMstListOutputData output)
    {
        Result.Data = new GetRaiinFilterMstListResponse(output.FilterMsts);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRaiinFilterMstListStatus status) => status switch
    {
        GetRaiinFilterMstListStatus.Success => ResponseMessage.Success,
        GetRaiinFilterMstListStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}

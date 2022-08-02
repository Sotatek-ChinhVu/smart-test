using Domain.Models.RaiinFilterMst;

namespace EmrCloudApi.Tenant.Responses.RaiinFilter;

public class GetRaiinFilterMstListResponse
{
    public GetRaiinFilterMstListResponse(List<RaiinFilterMstModel> filters)
    {
        FilterMsts = filters;
    }

    public List<RaiinFilterMstModel> FilterMsts { get; private set; }
}

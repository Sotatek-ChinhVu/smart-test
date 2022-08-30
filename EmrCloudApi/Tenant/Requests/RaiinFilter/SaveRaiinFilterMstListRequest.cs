using Domain.Models.RaiinFilterMst;

namespace EmrCloudApi.Tenant.Requests.RaiinFilter;

public class SaveRaiinFilterMstListRequest
{
    public List<RaiinFilterMstModel> FilterMsts { get; set; } = null!;
}

using Domain.Models.RaiinFilterMst;

namespace EmrCloudApi.Requests.RaiinFilter;

public class SaveRaiinFilterMstListRequest
{
    public List<RaiinFilterMstModel> FilterMsts { get; set; } = null!;
}

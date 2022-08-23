using UseCase.SetMst.ReorderSetMstList;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class ReorderSetMstRequest
{
    public List<ReorderSetMstInputItem> SetMstLists { get; set; } = new();
}

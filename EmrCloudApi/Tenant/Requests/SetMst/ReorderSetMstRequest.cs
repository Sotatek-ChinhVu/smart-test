using UseCase.SetMst.ReorderSetMstList;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class ReorderSetMstRequest
{
    public ReorderSetMstRequestItem DragSetMstItem { get; set; } = new ReorderSetMstRequestItem();
    public ReorderSetMstRequestItem DropSetMstItem { get; set; } = new ReorderSetMstRequestItem();
}
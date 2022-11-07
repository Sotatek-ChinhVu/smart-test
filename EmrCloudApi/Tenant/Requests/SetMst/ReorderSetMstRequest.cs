namespace EmrCloudApi.Tenant.Requests.SetMst;

public class ReorderSetMstRequest
{
    public int HpId { get; set; }

    public int DragSetCd { get; set; }

    public int DropSetCd { get; set; }
}
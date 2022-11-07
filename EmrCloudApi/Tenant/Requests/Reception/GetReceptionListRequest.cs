using Helper.Constants;

namespace EmrCloudApi.Tenant.Requests.Reception;

public class GetReceptionListRequest
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
}

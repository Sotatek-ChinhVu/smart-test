using Helper.Constants;

namespace EmrCloudApi.Tenant.Messages;

public class CommonMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
}

using Helper.Constants;

namespace EmrCloudApi.Tenant.Requests.Reception
{
    public class GetReceptionLockRequest
    {
        public int HpId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; } = CommonConstants.InvalidId;
        public long PtId { get; set; } = CommonConstants.InvalidId;
    }
}

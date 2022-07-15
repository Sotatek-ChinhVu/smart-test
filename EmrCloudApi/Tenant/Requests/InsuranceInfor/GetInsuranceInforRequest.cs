using Domain.CommonObject;

namespace EmrCloudApi.Tenant.Requests.InsuranceInfor
{
    public class GetInsuranceInforRequest
    {
        public long PtId { get; set; }
        public int HokenId { get; set; }
        public GetInsuranceInforRequest(long ptId, int hokenId)
        {
            PtId = ptId;
            HokenId = hokenId;
        }
    }
}

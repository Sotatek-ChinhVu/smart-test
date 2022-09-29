using Domain.Models.ReceptionSameVisit;

namespace EmrCloudApi.Tenant.Requests.ReceptionVisiting
{
    public class GetReceptionVisitingRequest
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
    }
}

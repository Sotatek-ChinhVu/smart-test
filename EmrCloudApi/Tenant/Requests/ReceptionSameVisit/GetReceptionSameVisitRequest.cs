using Domain.Models.ReceptionSameVisit;

namespace EmrCloudApi.Tenant.Requests.ReceptionSameVisit
{
    public class GetReceptionSameVisitRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }
    }
}

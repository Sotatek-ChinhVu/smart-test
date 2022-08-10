using Domain.Models.ReceptionSameVisit;

namespace EmrCloudApi.Tenant.Requests.ReceptionSameVisit
{
    public class GetReceptionSameVisitRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int UserIdDoctor { get; set; }
    }
}

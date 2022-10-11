using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Tenant.Requests.RaiinKubun
{
    public class RaiinKbnYayokuRequest
    {
        public int HpId { get; set; }

        public int GrpId { get; set; }

        public int KbnCd { get; set; }

        public long SeqNo { get; set; }

        public int YoyakuCd { get; set; }

        public int IsDeleted { get; set; }

        public RaiinKbnYayokuModel Map()
        {
            return new RaiinKbnYayokuModel(HpId, KbnCd, SeqNo, YoyakuCd, IsDeleted);
        }
    }
}

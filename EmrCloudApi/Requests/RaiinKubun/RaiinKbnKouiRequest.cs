using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Requests.RaiinKubun
{
    public class RaiinKbnKouiRequest
    {
        public int HpId { get;  set; }

        public int GrpId { get;  set; }

        public int KbnCd { get;  set; }

        public int SeqNo { get;  set; }

        public int KouiKbnId { get;  set; }

        public int IsDeleted { get;  set; }

        public RaiinKbnKouiModel Map()
        {
            return new RaiinKbnKouiModel(HpId, GrpId, KbnCd,SeqNo, KouiKbnId, IsDeleted);
        }

    }
}

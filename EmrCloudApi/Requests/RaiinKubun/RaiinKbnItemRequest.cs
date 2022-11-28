using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Requests.RaiinKubun
{
    public class RaiinKbnItemRequest
    {
        public int HpId { get;  set; }

        public int GrpCd { get;  set; }

        public int KbnCd { get;  set; }

        public long SeqNo { get;  set; }

        public string ItemCd { get;  set; } = String.Empty;

        public int IsExclude { get;  set; }

        public int IsDeleted { get;  set; }

        public int SortNo { get;  set; }

        public RaiinKbnItemModel Map()
        {
            return new RaiinKbnItemModel(HpId, GrpCd, KbnCd, SeqNo, ItemCd, IsExclude, IsDeleted, SortNo);
        }
    }
}

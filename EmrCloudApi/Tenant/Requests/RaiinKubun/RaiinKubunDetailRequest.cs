using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Tenant.Requests.RaiinKubun
{
    public class RaiinKubunDetailRequest
    {
        public int HpId { get; set; }

        public int GroupId { get; set; }

        public int KubunCd { get; set; }

        public int SortNo { get; set; }

        public string KubunName { get; set; } = String.Empty;

        public string ColorCd { get; set; } = String.Empty;

        public bool IsConfirmed { get; set; }

        public bool IsAuto { get; set; }

        public bool IsAutoDeleted { get; set; }

        public bool IsDeleted { get; set; }

        public List<RaiinKbnKouiRequest> RaiinKbnKouiModels { get; set; } = new List<RaiinKbnKouiRequest>();

        public List<RaiinKbnItemRequest> RaiinKbnItemModels { get; set; } = new List<RaiinKbnItemRequest>();

        public List<RaiinKbnYayokuRequest> RaiinKbnYayokuModels { get; set; } = new List<RaiinKbnYayokuRequest>();

        public RaiinKubunDetailModel Map()
        {
            return new RaiinKubunDetailModel(HpId, GroupId, KubunCd, SortNo, KubunName, ColorCd, IsConfirmed, IsAuto, IsAutoDeleted, IsDeleted, RaiinKbnKouiModels.Select(x => x.Map()).ToList(), RaiinKbnItemModels.Select(x => x.Map()).ToList(),new List<RsvFrameMstModel>(),new List<RsvGrpMstModel>(), RaiinKbnYayokuModels.Select(x => x.Map()).ToList());
        }
    }
}

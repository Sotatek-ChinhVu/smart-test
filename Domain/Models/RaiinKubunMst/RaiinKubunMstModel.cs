
namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKubunMstModel
    {
        public int GroupId { get; private set; }

        public int SortNo { get; private set; }

        public string GroupName { get; private set; }

        public bool IsDeleted { get; private set; }

        public List<RaiinKubunDetailModel> RaiinKubunDetailModels { get; private set; }
        public List<RaiinKbnKouiModel> RaiinKbnKouiModels { get; private set; }
        public List<RaiinKbnItemModel> RaiinKbnItemModels { get; private set; }
        public List<RsvFrameMstModel> RsvFrameMstModels { get; private set; }
        public List<RsvGrpMstModel> RsvGrpMstModels { get; private set; }
        public List<RaiinKbnYayokuModel> RaiinKbnYayokuModels { get; private set; }

        public RaiinKubunMstModel(int groupId, int sortNo, string groupName, bool isDeleted, List<RaiinKubunDetailModel> raiinKubunDetailModels, List<RaiinKbnKouiModel> raiinKbnKouiModels, List<RaiinKbnItemModel> raiinKbnItemModels, List<RsvFrameMstModel> rsvFrameMstModels, List<RsvGrpMstModel> rsvGrpMstModels, List<RaiinKbnYayokuModel> raiinKbnYayokuModels)
        {
            GroupId = groupId;
            SortNo = sortNo;
            GroupName = groupName;
            IsDeleted = isDeleted;
            RaiinKubunDetailModels = raiinKubunDetailModels;
            RaiinKbnKouiModels = raiinKbnKouiModels;
            RaiinKbnItemModels = raiinKbnItemModels;
            RsvFrameMstModels = rsvFrameMstModels;
            RsvGrpMstModels = rsvGrpMstModels;
            RaiinKbnYayokuModels = raiinKbnYayokuModels;
        }
    }
}

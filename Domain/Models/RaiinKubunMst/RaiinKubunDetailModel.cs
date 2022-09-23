namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKubunDetailModel
    {
        public int HpId { get; private set; }
        public int GroupId { get; private set; }

        public int KubunCd { get; private set; }

        public int SortNo { get; private set; }

        public string KubunName { get; private set; }

        public string ColorCd { get; private set; }

        public bool IsConfirmed { get; private set; }

        public bool IsAuto { get; private set; }

        public bool IsAutoDeleted { get; private set; }

        public bool IsDeleted { get; private set; }
        public List<RaiinKbnKouiModel> RaiinKbnKouiModels { get; private set; }
        public List<RaiinKbnItemModel> RaiinKbnItemModels { get; private set; }
        public List<RsvFrameMstModel> RsvFrameMstModels { get; private set; }
        public List<RsvGrpMstModel> RsvGrpMstModels { get; private set; }
        public List<RaiinKbnYayokuModel> RaiinKbnYayokuModels { get; private set; }

        public RaiinKubunDetailModel(int hpId,int groupId, int kubunCd, int sortNo, string kubunName, string colorCd, bool isConfirmed, bool isAuto, bool isAutoDeleted, bool isDeleted, List<RaiinKbnKouiModel> raiinKbnKouiModels, List<RaiinKbnItemModel> raiinKbnItemModels, List<RsvFrameMstModel> rsvFrameMstModels, List<RsvGrpMstModel> rsvGrpMstModels, List<RaiinKbnYayokuModel> raiinKbnYayokuModels)
        {
            HpId = hpId;
            GroupId = groupId;
            KubunCd = kubunCd;
            SortNo = sortNo;
            KubunName = kubunName;
            ColorCd = colorCd;
            IsConfirmed = isConfirmed;
            IsAuto = isAuto;
            IsAutoDeleted = isAutoDeleted;
            IsDeleted = isDeleted;
            RaiinKbnKouiModels = raiinKbnKouiModels;
            RaiinKbnItemModels = raiinKbnItemModels;
            RsvFrameMstModels = rsvFrameMstModels;
            RsvGrpMstModels = rsvGrpMstModels;
            RaiinKbnYayokuModels = raiinKbnYayokuModels;
        }
    }
}

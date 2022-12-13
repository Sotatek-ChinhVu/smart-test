namespace Domain.Models.PtGroupMst
{
    public class GroupNameMstModel
    {
        public GroupNameMstModel(int grpId, int sortNo, string grpName,int isDeleted, List<GroupItemModel> groupItems)
        {
            GrpId = grpId;
            SortNo = sortNo;
            GrpName = grpName;
            IsDeleted = isDeleted;
            GroupItems = groupItems;
        }

        public int GrpId { get; private set; }

        public int SortNo { get; private set; }

        public string GrpName { get; private set; }

        public int IsDeleted { get; private set; }

        public List<GroupItemModel> GroupItems { get; private set; } = new List<GroupItemModel>();
    }
}

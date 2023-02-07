using Domain.Models.PtGroupMst;

namespace EmrCloudApi.Responses.PtGroupMst
{
    public class GroupNameDtoResponse
    {
        public GroupNameDtoResponse(GroupNameMstModel model)
        {
            GrpId = model.GrpId;
            SortNo = model.SortNo;
            GrpName = model.GrpName;
            IsDeleted = model.IsDeleted;
            GroupItems = model.GroupItems.Select(x => new GroupItemDtoResponse(x)).ToList();
        }

        public int GrpId { get; private set; }

        public int SortNo { get; private set; }

        public string GrpName { get; private set; } = string.Empty;

        public int IsDeleted { get; private set; }

        public List<GroupItemDtoResponse> GroupItems { get; private set; }
    }
}

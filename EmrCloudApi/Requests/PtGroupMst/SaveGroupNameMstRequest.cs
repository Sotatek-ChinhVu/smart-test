using Domain.Models.PtGroupMst;

namespace EmrCloudApi.Requests.PtGroupMst
{
    public class SaveGroupNameMstRequest
    {
        public SaveGroupNameMstRequest(List<GroupNameMstModel> groupNameMsts)
        {
            GroupNameMsts = groupNameMsts;
        }

        public List<GroupNameMstModel> GroupNameMsts { get; private set; }
    }
}

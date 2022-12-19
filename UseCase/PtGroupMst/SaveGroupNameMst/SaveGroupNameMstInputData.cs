using Domain.Models.PtGroupMst;
using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.SaveGroupNameMst
{
    public class SaveGroupNameMstInputData : IInputData<SaveGroupNameMstOutputData>
    {
        public SaveGroupNameMstInputData(int userId, int hpId, List<GroupNameMstModel> groupNameMst)
        {
            UserId = userId;
            HpId = hpId;
            GroupNameMst = groupNameMst;
        }

        public int UserId { get; private set; }

        public int HpId { get; private set; }

        public List<GroupNameMstModel> GroupNameMst { get; private set; }
    }
}

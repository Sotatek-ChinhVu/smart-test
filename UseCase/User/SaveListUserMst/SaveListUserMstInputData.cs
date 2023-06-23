using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.SaveListUserMst
{
    public class SaveListUserMstInputData : IInputData<SaveListUserMstOutputData>
    {
        public SaveListUserMstInputData(int hpId, List<UserMstModel> users, int userId)
        {
            HpId = hpId;
            Users = users;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public List<UserMstModel> Users { get; private set; }

        public int UserId { get; private set; }
    }
}

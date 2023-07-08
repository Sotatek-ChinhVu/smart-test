using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListUserByCurrentUser
{
    public class GetListUserByCurrentUserOutputData : IOutputData
    {
        public GetListUserByCurrentUserOutputData(GetListUserByCurrentUserStatus status, List<UserMstModel> users, bool getShowRenkeiCd1ColumnSetting, int managerKbnCurrrentUser)
        {
            Status = status;
            Users = users;
            GetShowRenkeiCd1ColumnSetting = getShowRenkeiCd1ColumnSetting;
            ManagerKbnCurrrentUser = managerKbnCurrrentUser;
        }

        public GetListUserByCurrentUserStatus Status { get; private set; }

        public List<UserMstModel> Users { get; private set; }

        public bool GetShowRenkeiCd1ColumnSetting { get; private set; }

        public int ManagerKbnCurrrentUser { get; private set; }
    }
}

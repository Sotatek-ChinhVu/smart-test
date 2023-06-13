using Domain.Models.User;

namespace EmrCloudApi.Responses.User
{
    public class GetListUserByCurrentUserResponse
    {
        public GetListUserByCurrentUserResponse(List<UserMstModel> users, bool getShowRenkeiCd1ColumnSetting)
        {
            Users = users;
            GetShowRenkeiCd1ColumnSetting = getShowRenkeiCd1ColumnSetting;
        }

        public List<UserMstModel> Users { get; private set; }

        public bool GetShowRenkeiCd1ColumnSetting { get; private set; }
    }
}

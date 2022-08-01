using Domain.Models.User;

namespace EmrCloudApi.Tenant.Responses.User
{
    public class GetUserListResponse
    {
        public List<UserMstModel> UserList { get; set; } = new List<UserMstModel>();
    }
}

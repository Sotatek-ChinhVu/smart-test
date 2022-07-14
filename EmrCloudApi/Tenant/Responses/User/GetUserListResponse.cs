using Domain.Models.User;

namespace EmrCloudApi.Tenant.Responses.User
{
    public class GetUserListResponse
    {
        public List<UserMst> UserList { get; set; } = new List<UserMst>();
    }
}

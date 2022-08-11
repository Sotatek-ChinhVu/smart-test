using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetList;

public class GetUserListOutputData : IOutputData
{
    public GetUserListOutputData(GetUserListStatus status)
    {
        Status = status;
    }

    public GetUserListOutputData(GetUserListStatus status, List<UserMstModel> users)
    {
        Status = status;
        Users = users;
    }

    public GetUserListStatus Status { get; private set; }
    public List<UserMstModel> Users { get; private set; } = new List<UserMstModel>();
}

using Domain.Models.User;
using UseCase.Core.Sync.Core;
using UseCase.User.UpsertList;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    public UpsertUserListInputData(List<UserMstModel> upsertUserList, int userId)
    {
        UpsertUserList = upsertUserList;
        UserId = userId;
    }

    public List<UserMstModel> UpsertUserList { get; private set; }

    public int UserId { get; private set; }

    public List<UserMstModel> ToList()
    {
        return UpsertUserList;
    }
}

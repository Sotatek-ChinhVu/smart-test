using Domain.Models.User;
using UseCase.Core.Sync.Core;
using UseCase.User.UpsertList;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    public UpsertUserListInputData(List<UserMstModel> upsertUserList)
    {
        this.UpsertUserList = upsertUserList;
    }

    public List<UserMstModel> UpsertUserList { get; private set; }

    public List<UserMstModel> ToList()
    {
        return this.UpsertUserList;
    }
}

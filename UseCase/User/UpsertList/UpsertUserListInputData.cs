using Domain.Models.User;
using UseCase.Core.Sync.Core;
using UseCase.User.UpsertList;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    private List<UserMstModel> upsertUserList;

    public UpsertUserListInputData(List<UpsertUserListInputItem> UserMstModel)
    {
        this.UserMstModel = UserMstModel;
    }

    public UpsertUserListInputData(List<UserMstModel> upsertUserList)
    {
        this.upsertUserList = upsertUserList;
    }

    public List<UpsertUserListInputItem> UserMstModel { get; private set; }
    public List<UpsertUserListInputItem> ToList()
    {
        return this.UserMstModel;
    }
}

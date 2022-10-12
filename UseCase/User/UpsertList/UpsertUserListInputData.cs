using Domain.Models.User;
using UseCase.Core.Sync.Core;
using UseCase.User.UpsertList;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    public UpsertUserListInputData(List<UpsertUserListInputItem> upsertUserList)
    {
        this.UpsertUserList = upsertUserList;
    }

    public List<UpsertUserListInputItem> UpsertUserList { get; private set; }

    public List<UpsertUserListInputItem> ToList()
    {
        return this.UpsertUserList;
    }
}

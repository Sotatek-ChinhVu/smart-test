using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    public UpsertUserListInputData(List<UserMstModel> upsertUserList)
    {
        UpsertUserList = upsertUserList;
    }
    public List<UserMstModel> UpsertUserList { get; set; }
}

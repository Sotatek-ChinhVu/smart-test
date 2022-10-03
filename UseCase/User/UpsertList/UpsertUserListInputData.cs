using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList;

public class UpsertUserListInputData : IInputData<UpsertUserListOutputData>
{
    public UpsertUserListInputData(List<UserMstModel> updatedUserList/*, List<UserMstModel> inserteddUserList*/)
    {
        UpdatedUserList = updatedUserList;
        //InserteddUserList = inserteddUserList;
    }

    public List<UserMstModel> UpdatedUserList { get; set; }

    //public List<UserMstModel> InserteddUserList { get; set; }
}

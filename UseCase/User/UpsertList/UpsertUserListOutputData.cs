using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList;

public class UpsertUserListOutputData : IOutputData
{
    public UpsertUserListStatus Status { get; private set; }
    public UpsertUserListOutputData(UpsertUserListStatus status)
    {
        Status = status;
    } 
}

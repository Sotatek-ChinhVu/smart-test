using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList;

public class UpsertUserListOutputData : IOutputData
{
    public UpsertUserListOutputData(UpsertUserListStatus status)
    {
        Status = status;
    }

    public UpsertUserListStatus Status { get; private set; }
}

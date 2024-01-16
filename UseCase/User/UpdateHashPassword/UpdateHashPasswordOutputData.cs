using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpdateHashPassword;

public class UpdateHashPasswordOutputData : IOutputData
{
    public UpdateHashPasswordOutputData(UpdateHashPasswordStatus status)
    {
        Status = status;
    }

    public UpdateHashPasswordStatus Status { get; private set; }
}

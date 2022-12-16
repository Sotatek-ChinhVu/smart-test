using UseCase.Core.Sync.Core;

namespace UseCase.User.UpdateUserConf;

public class UpdateUserConfOutputData : IOutputData
{
    public UpdateUserConfOutputData(UpdateUserConfStatus status)
    {
        Status = status;
    }

    public UpdateUserConfStatus Status { get; private set; }
}

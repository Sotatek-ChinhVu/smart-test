using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.IsNyukinExisted;

public class IsNyukinExistedOutputData : IOutputData
{
    public IsNyukinExistedOutputData(IsNyukinExistedStatus status, bool success)
    {
        Status = status;
        Success = success;
    }

    public IsNyukinExistedStatus Status { get; private set; }

    public bool Success { get; private set; }
}

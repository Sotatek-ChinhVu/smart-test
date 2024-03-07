using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.RecalculateInSeikyuPending;

public class RecalculateInSeikyuPendingOutputData : IOutputData
{
    public RecalculateInSeikyuPendingOutputData(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}

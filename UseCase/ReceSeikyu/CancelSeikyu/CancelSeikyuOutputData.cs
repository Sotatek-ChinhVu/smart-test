using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.CancelSeikyu;

public class CancelSeikyuOutputData : IOutputData
{
    public CancelSeikyuOutputData(CancelSeikyuStatus status)
    {
        Status = status;
    }

    public CancelSeikyuStatus Status { get; private set; }
}

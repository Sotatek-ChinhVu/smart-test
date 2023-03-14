using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.Recalculation;

public class RecalculationOutputData : IOutputData
{
    public RecalculationOutputData(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}

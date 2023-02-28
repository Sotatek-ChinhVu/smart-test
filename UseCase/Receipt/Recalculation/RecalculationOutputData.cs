using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.Recalculation;

public class RecalculationOutputData : IOutputData
{
    public RecalculationOutputData(RecalculationStatus status, string errorMessage)
    {
        Status = status;
        ErrorMessage = errorMessage;
    }

    public RecalculationStatus Status { get; private set; }

    public string ErrorMessage { get; private set; }
}

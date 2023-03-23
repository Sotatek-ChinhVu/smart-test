using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceiptEdit;

public class SaveReceiptEditOutputData : IOutputData
{
    public SaveReceiptEditOutputData(SaveReceiptEditStatus status)
    {
        Status = status;
    }

    public SaveReceiptEditStatus Status { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.ReceiptEdit;

public class GetReceiptEditOutputData : IOutputData
{
    public GetReceiptEditOutputData(ReceiptEditItem receiptEditOrigin, ReceiptEditItem receiptEditCurrent, Dictionary<string, string> tokkiMstDictionary, GetReceiptEditStatus status)
    {
        ReceiptEditOrigin = receiptEditOrigin;
        ReceiptEditCurrent = receiptEditCurrent;
        TokkiMstDictionary = tokkiMstDictionary;
        Status = status;
    }

    public ReceiptEditItem ReceiptEditOrigin { get; private set; }

    public ReceiptEditItem ReceiptEditCurrent { get; private set; }

    public Dictionary<string, string> TokkiMstDictionary { get; private set; }

    public GetReceiptEditStatus Status { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceCheckCmtList;

public class SaveReceCheckCmtListOutputData : IOutputData
{
    public SaveReceCheckCmtListOutputData(SaveReceCheckCmtListStatus status, List<ReceiptCheckCmtErrListItem> receiptCheckCmtErrList)
    {
        Status = status;
        ReceiptCheckCmtErrList = receiptCheckCmtErrList;
    }

    public SaveReceCheckCmtListStatus Status { get; private set; }

    public List<ReceiptCheckCmtErrListItem> ReceiptCheckCmtErrList { get; private set; }
}

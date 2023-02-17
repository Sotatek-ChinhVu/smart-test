using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceiCheckList;

public class GetReceiCheckListOutputData : IOutputData
{
    public GetReceiCheckListOutputData(List<ReceiptCheckCmtErrListItem> receiptCheckCmtErrList, GetReceiCheckListStatus status)
    {
        ReceiptCheckCmtErrList = receiptCheckCmtErrList;
        Status = status;
    }

    public List<ReceiptCheckCmtErrListItem> ReceiptCheckCmtErrList { get; private set; }

    public GetReceiCheckListStatus Status { get; private set; }
}

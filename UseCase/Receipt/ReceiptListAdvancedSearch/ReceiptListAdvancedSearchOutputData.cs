using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.ReceiptListAdvancedSearch;

public class ReceiptListAdvancedSearchOutputData : IOutputData
{
    public ReceiptListAdvancedSearchOutputData(List<ReceiptListModel> receiptList, ReceiptListAdvancedSearchStatus status)
    {
        ReceiptList = receiptList.Select(item => new ReceiptListAdvancedSearchItem(item)).ToList();
        Status = status;
    }

    public ReceiptListAdvancedSearchOutputData(ReceiptListAdvancedSearchStatus status)
    {
        ReceiptList = new();
        Status = status;
    }

    public List<ReceiptListAdvancedSearchItem> ReceiptList { get; private set; }

    public ReceiptListAdvancedSearchStatus Status { get; private set; }
}

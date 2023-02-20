using UseCase.Receipt.ReceiptListAdvancedSearch;

namespace EmrCloudApi.Responses.Receipt;

public class ReceiptListAdvancedSearchResponse
{
    public ReceiptListAdvancedSearchResponse(List<ReceiptListAdvancedSearchItem> receiptList)
    {
        ReceiptList = receiptList;
    }

    public List<ReceiptListAdvancedSearchItem> ReceiptList { get; private set; }
}

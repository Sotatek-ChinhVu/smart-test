using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class ReceiptListAdvancedSearchResponse
{
    public ReceiptListAdvancedSearchResponse(List<ReceiptListAdvancedSearchOutputItem> receiptList)
    {
        ReceiptList = receiptList.Select(item => new ReceiptListModelDto(item)).ToList();
    }

    public List<ReceiptListModelDto> ReceiptList { get; private set; }
}

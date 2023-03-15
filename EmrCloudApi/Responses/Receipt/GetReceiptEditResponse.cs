using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceiptEditResponse
{
    public GetReceiptEditResponse(ReceiptEditItem receiptEditOrigin, ReceiptEditItem receiptEditCurrent, Dictionary<string, string> tokkiMstDictionary)
    {
        ReceiptEditOrigin = new ReceiptEditDto(receiptEditOrigin);
        ReceiptEditCurrent = new ReceiptEditDto(receiptEditCurrent);
        TokkiMstDictionary = tokkiMstDictionary;
    }

    public ReceiptEditDto ReceiptEditOrigin { get; private set; }

    public ReceiptEditDto ReceiptEditCurrent { get; private set; }

    public Dictionary<string, string> TokkiMstDictionary { get; private set; }
}

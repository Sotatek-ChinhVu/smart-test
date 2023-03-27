using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceiptEditResponse
{
    public GetReceiptEditResponse(int seqNo, ReceiptEditItem receiptEditOrigin, ReceiptEditItem receiptEditCurrent, Dictionary<string, string> tokkiMstDictionary)
    {
        SeqNo = seqNo;
        ReceiptEditOrigin = new ReceiptEditDto(receiptEditOrigin);
        ReceiptEditCurrent = new ReceiptEditDto(receiptEditCurrent);
        TokkiMstDictionary = tokkiMstDictionary;
    }

    public int SeqNo { get; private set; }

    public ReceiptEditDto ReceiptEditOrigin { get; private set; }

    public ReceiptEditDto ReceiptEditCurrent { get; private set; }

    public Dictionary<string, string> TokkiMstDictionary { get; private set; }
}

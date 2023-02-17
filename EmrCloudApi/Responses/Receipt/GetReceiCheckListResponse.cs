using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceiCheckListResponse
{
    public GetReceiCheckListResponse(List<ReceiptCheckCmtErrListItem> receiptCheckCmtErrList)
    {
        ReceiptCheckCmtErrList = receiptCheckCmtErrList.Select(item => new ReceiptCheckCmtErrDto(item)).ToList();
    }

    public List<ReceiptCheckCmtErrDto> ReceiptCheckCmtErrList { get; private set; }
}

using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetRecePreviewList;

public class GetRecePreviewListInputData : IInputData<GetRecePreviewListOutputData>
{
    public GetRecePreviewListInputData(int hpId, ReceiptPreviewModeEnum receiptPreviewType, long ptId)
    {
        HpId = hpId;
        ReceiptPreviewType = receiptPreviewType;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public ReceiptPreviewModeEnum ReceiptPreviewType { get; private set; }

    public long PtId { get; private set; }
}

using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetRecePreviewList;

public class GetRecePreviewListInputData : IInputData<GetRecePreviewListOutputData>
{
    public GetRecePreviewListInputData(int hpId, ReceiptPreviewModeEnum typeReceiptPreview, long ptId)
    {
        HpId = hpId;
        TypeReceiptPreview = typeReceiptPreview;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public ReceiptPreviewModeEnum TypeReceiptPreview { get; private set; }

    public long PtId { get; private set; }
}

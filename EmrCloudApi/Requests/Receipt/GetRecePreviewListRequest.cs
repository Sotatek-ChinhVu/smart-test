using Helper.Enum;

namespace EmrCloudApi.Requests.Receipt;

public class GetRecePreviewListRequest
{
    public long PtId { get; set; }

    public ReceiptPreviewModeEnum TypeReceiptPreview { get; set; }
}

using Helper.Enum;

namespace EmrCloudApi.Requests.Receipt;

public class GetRecePreviewListRequest
{
    public long PtId { get; set; }

    public ReceiptPreviewModeEnum ReceiptPreviewType { get; set; }
}

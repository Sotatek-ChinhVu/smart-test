namespace EmrCloudApi.Requests.ExportPDF;

public class GetReceiptListRequest
{
    public int HpId { get; set; }

    public int SeikyuYm { get; set; }

    public List<ReceiptListRequestItem> ReceiptListModels { get; set; } = new();
}

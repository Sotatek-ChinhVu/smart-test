namespace EmrCloudApi.Requests.ExportPDF;

public class GetReceiptListRequest : ReportRequestBase
{
    public int SeikyuYm { get; set; }

    public List<ReceiptListRequestItem> ReceiptListModels { get; set; } = new();
}

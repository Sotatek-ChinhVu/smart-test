namespace EmrCloudApi.Requests.ExportPDF;

public class OrderLabelExportRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public List<KouiKbnModel> OdrKouiKbns { get; set; } = new();
}

public class KouiKbnModel
{
    public int From { get; set; }

    public int To { get; set; }
}
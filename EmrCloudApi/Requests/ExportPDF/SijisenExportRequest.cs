namespace EmrCloudApi.Requests.ExportPDF;

public class SijisenExportRequest
{
    public int FormType { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public List<LimitModel> OdrKouiKbns { get; set; } = new();

    public bool PrintNoOdr { get; set; }
}

public class LimitModel
{
    public int From { get; set; }

    public int To { get; set; }
}

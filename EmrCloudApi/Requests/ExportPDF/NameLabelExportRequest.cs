namespace EmrCloudApi.Requests.ExportPDF;

public class NameLabelExportRequest
{
    public long PtId { get; set; }

    public string KanjiName { get; set; } = string.Empty;

    public int SinDate { get; set; }
}

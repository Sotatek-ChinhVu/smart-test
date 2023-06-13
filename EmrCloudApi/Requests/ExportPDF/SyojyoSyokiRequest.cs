namespace EmrCloudApi.Requests.ExportPDF;

public class SyojyoSyokiRequest : ReportRequestBase
{
    public long PtId { get; set; }

    public int SeiKyuYm { get; set; }

    public int HokenId { get; set; }
}

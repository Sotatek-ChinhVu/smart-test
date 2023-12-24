namespace EmrCloudApi.Requests.ExportPDF;

public class Karte1ExportRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public int HokenPid { get; set; }

    public bool TenkiByomei { get; set; }

    public bool SyuByomei { get; set; }
}

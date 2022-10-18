namespace EmrCloudApi.Tenant.Requests.ExportPDF;

public class Karte1ExportRequest
{
    public int HpId { get; set; }
    public long PtId { get; set; }
    public int SinDate { get; set; }
    public int HokenPid { get; set; }
    public bool TenkiByomei { get; set; }
}

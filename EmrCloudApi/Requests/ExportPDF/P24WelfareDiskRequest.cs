namespace EmrCloudApi.Requests.ExportPDF;

public class P24WelfareDiskRequest : ReportRequestBase
{
    public int SeikyuYm { get; set; }

    public bool IsNormal { get; set; }

    public bool IsPaper { get; set; }

    public bool IsDelay { get; set; }

    public bool IsHenrei { get; set; }

    public bool IsOnline { get; set; }
}

namespace EmrCloudApi.Tenant.Responses.ExportPDF;

public class Karte1ExportResponse
{
    public Karte1ExportResponse(string urlPdf)
    {
        UrlPdf = urlPdf;
    }

    public string UrlPdf { get; private set; }
}

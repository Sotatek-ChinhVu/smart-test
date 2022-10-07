namespace EmrCloudApi.Tenant.Responses.ExportPDF;

public class Karte1ExportResponse
{
    public Karte1ExportResponse(string base64String)
    {
        Base64String = base64String;
    }

    public string Base64String { get; private set; }
}

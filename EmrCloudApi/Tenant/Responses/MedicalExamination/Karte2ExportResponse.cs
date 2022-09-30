namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class Karte2ExportResponse
    {
        public string Url { get; private set; }

        public Karte2ExportResponse(string url)
        {
            Url = url;
        }
    }
}

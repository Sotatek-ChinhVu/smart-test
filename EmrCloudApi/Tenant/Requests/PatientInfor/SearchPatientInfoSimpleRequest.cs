namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SearchPatientInfoSimpleRequest
    {
        public string Keyword { get; set; } = string.Empty;

        public bool IsContainMode { get; set; } = false;
    }
}

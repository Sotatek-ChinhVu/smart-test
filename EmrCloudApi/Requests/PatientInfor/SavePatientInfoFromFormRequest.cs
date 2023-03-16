namespace EmrCloudApi.Requests.PatientInfor
{
    public class SavePatientInfoFromFormRequest
    {
        public string JsonPt { get; set; } = string.Empty;

        public IList<SaveInsuranceScanRequest> ImageScans { get; set; } = new List<SaveInsuranceScanRequest>();
    }
}

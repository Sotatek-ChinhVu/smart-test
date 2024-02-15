namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class PatientStatusRequest
    {
        public string StatusLabel { get; set; } = string.Empty;

        public int StatusValue { get; set; }
    }
}

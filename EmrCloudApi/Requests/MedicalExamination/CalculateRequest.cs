namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CalculateRequest
    {
        public bool FromRcCheck { get; set; }

        public bool IsSagaku { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int SeikyuUp { get; set; }

        public string Prefix { get; set; } = string.Empty;
    }
}

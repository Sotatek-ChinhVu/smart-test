namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ConvertInputItemToTodayOrd
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public Dictionary<string, string> DetailInfs { get; set; } = new();
    }
}

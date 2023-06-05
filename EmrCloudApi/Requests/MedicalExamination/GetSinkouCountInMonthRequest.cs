namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetSinkouCountInMonthRequest
    {
        public int SinDate { get; set; }

        public long PtId { get; set; }

        public string ItemCd { get; set; } = string.Empty;
    }
}

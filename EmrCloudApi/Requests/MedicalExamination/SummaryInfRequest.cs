using Helper.Enum;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class SummaryInfRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public InfoType InfoType { get; set; }
    }
}

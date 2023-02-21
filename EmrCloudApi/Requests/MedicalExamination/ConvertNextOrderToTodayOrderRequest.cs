using UseCase.NextOrder;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ConvertNextOrderToTodayOrderRequest
    {
        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long PtId { get; set; }

        public List<RsvKrtOrderInfItem> rsvKrtOrderInfItems { get; set; } = new();
    }
}

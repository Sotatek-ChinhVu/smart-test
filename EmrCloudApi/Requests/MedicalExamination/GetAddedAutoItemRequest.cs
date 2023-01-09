using UseCase.MedicalExamination.GetAddedAutoItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetAddedAutoItemRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public List<OrderInfItem> OrderInfItems { get; set; } = new();
        public List<CurrentOrderInf> CurrentOrderInfs { get; set; } = new();
    }
}

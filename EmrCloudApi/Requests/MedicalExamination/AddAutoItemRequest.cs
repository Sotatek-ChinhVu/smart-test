using UseCase.MedicalExamination.AddAutoItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class AddAutoItemRequest
    {
        public int SinDate { get; set; }
        public List<AddedOrderInf> AddedOrderInfs { get; set; } = new();
        public List<OrderInfItem> OrderInfItems { get; set; } = new();
    }
}

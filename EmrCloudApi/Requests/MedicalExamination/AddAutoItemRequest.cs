using UseCase.MedicalExamination.AddAutoItem;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class AddAutoItemRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public int SinDate { get; set; }
        public List<AddedOrderInf> AddedOrderInfs { get; set; } = new();
        public List<OrderInfItem> OrderInfItems { get; set; } = new();
    }
}

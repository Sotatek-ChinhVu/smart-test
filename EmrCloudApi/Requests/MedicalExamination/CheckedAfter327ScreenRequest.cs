using Domain.Models.MedicalExamination;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CheckedAfter327ScreenRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public List<CheckedOrderModel> CheckedOrderModels { get; set; } = new();

        public bool IsTokysyoOrder { get; set; }

        public bool IsTokysyosenOrder { get; set; }
    }
}

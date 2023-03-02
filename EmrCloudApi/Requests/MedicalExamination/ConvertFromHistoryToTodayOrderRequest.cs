using Domain.Models.OrdInfs;

namespace EmrCloudApi.Requests.MedicalExamination
{
    public class ConvertFromHistoryToTodayOrderRequest
    {
        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long PtId { get; set; }

        public List<OrdInfModel> HistoryOdrInfModels { get; set; } = new();
    }
}

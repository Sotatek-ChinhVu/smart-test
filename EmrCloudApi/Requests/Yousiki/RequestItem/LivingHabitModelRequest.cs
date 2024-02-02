namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class LivingHabitModelRequest
    {
        public List<UpdateYosiki1InfDetailRequestItem> UpdateYosiki1InfDetailRequestItems { get; set; } = new();

        public List<OutpatientConsultationInfModelRequest> OutpatientConsultationInfList { get; set; } = new();

        public List<StrokeHistoryModelRequest> StrokeHistoryList { get; set; } = new();

        public List<AcuteCoronaryHistoryModelRequest> AcuteCoronaryHistoryList { get; set; } = new();

        public List<AcuteAorticDissectionHistoryModelRequest> AcuteAorticHistoryList { get; set; } = new();

        public Dictionary<UpdateYosiki1InfDetailRequestItem, int> ValueSelectObjectRequest { get; set; } = new();
    }
}

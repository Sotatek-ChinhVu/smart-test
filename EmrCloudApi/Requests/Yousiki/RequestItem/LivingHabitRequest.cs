namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class LivingHabitRequest
    {
        public List<Yousiki1InfDetailRequest> Yousiki1InfDetails { get; set; } = new();

        public List<OutpatientConsultationInfRequest> OutpatientConsultationInfs { get; set; } = new();

        public List<StrokeHistoryRequest> StrokeHistorys { get; set; } = new();

        public List<AcuteCoronaryHistoryRequest> AcuteCoronaryHistorys { get; set; } = new();

        public List<AcuteAorticDissectionHistoryRequest> AcuteAorticHistorys { get; set; } = new();
    }
}

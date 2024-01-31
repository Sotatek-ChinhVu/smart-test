namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StrokeHistoryModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem Type { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem OnsetDate { get; set; } = new();
    }
}

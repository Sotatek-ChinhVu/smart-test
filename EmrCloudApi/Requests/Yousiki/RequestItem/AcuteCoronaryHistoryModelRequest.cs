using Domain.Models.Yousiki;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AcuteCoronaryHistoryModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem Type { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem OnsetDate { get; set; } = new();
    }
}

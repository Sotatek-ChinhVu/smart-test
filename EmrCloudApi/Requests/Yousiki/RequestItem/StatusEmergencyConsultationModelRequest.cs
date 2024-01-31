using Domain.Models.Yousiki;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusEmergencyConsultationModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem EmergencyConsultationDay { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem Destination { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem ConsultationRoute { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem OutCome { get; set; } = new();

        public bool IsDeleted { get; private set; }
    }
}

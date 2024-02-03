namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusEmergencyConsultationRequest
    {
        public Yousiki1InfDetailRequest EmergencyConsultationDay { get; set; } = new();

        public Yousiki1InfDetailRequest Destination { get; set; } = new();

        public Yousiki1InfDetailRequest ConsultationRoute { get; set; } = new();

        public Yousiki1InfDetailRequest OutCome { get; set; } = new();

        public bool IsDeleted { get; set; }
    }
}

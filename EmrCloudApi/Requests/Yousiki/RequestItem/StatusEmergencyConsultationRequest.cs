namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusEmergencyConsultationRequest
    {
        public Yousiki1InfDetailRequest? EmergencyConsultationDay { get; set; } 

        public Yousiki1InfDetailRequest? Destination { get; set; }

        public Yousiki1InfDetailRequest? ConsultationRoute { get; set; } 

        public Yousiki1InfDetailRequest? OutCome { get; set; } 

        public bool IsDeleted { get; set; }
    }
}

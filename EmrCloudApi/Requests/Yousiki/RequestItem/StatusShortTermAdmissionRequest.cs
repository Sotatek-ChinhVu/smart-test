namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusShortTermAdmissionRequest
    {
        public Yousiki1InfDetailRequest? AdmissionDate { get; set; } 

        public Yousiki1InfDetailRequest? DischargeDate { get; set; } 

        public Yousiki1InfDetailRequest? Service { get; set; } 

        public bool IsDeleted { get; private set; }
    }
}

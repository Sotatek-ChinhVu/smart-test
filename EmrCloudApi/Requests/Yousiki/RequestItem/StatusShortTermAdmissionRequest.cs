namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusShortTermAdmissionRequest
    {
        public Yousiki1InfDetailRequest AdmissionDate { get; set; } = new();

        public Yousiki1InfDetailRequest DischargeDate { get; set; } = new();

        public Yousiki1InfDetailRequest Service { get; set; } = new();

        public bool IsDeleted { get; private set; }
    }
}

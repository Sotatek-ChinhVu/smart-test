namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusVisitRequest
    {
        public Yousiki1InfDetailRequest SinDate { get; set; } = new();

        public Yousiki1InfDetailRequest MedicalInstitution { get; set; } = new();

        public bool IsDeleted { get; private set; }
    }
}

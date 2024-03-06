namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusVisitRequest
    {
        public Yousiki1InfDetailRequest? SinDate { get; set; } 

        public Yousiki1InfDetailRequest? MedicalInstitution { get; set; } 

        public bool IsDeleted { get; private set; }
    }
}

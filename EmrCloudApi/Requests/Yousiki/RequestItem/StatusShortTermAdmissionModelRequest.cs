namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusShortTermAdmissionModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem AdmissionDate { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem DischargeDate { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem Service { get; set; } = new();

        public bool IsDeleted { get; private set; }
    }
}

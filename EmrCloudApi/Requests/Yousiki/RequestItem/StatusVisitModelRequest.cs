using Domain.Models.Yousiki;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class StatusVisitModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem SinDate { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem MedicalInstitution { get; set; } = new();

        public bool IsDeleted { get; set; }
    }
}

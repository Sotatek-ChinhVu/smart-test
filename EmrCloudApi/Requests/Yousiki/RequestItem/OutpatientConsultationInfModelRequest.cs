using Domain.Models.Yousiki;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class OutpatientConsultationInfModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem ConsultationDate { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem FirstVisit { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem AppearanceReferral { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem DepartmentCode { get; set; } = new();

        public bool IsDeleted { get; private set; }
    }
}

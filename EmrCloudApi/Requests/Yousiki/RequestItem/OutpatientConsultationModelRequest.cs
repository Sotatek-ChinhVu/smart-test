using Domain.Models.Yousiki;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class OutpatientConsultationModelRequest
    {
        public UpdateYosiki1InfDetailRequestItem ConsultationDate { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem FirstVisit { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem Referral { get; set; } = new();

        public UpdateYosiki1InfDetailRequestItem DepartmentCode { get; set; } = new();
    }
}

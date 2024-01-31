using Domain.Models.Yousiki.CommonModel.CommonOutputModel;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AtHomeModelRequest
    {
        public List<StatusVisitModelRequest> StatusVisitList { get; set; } = new();

        public List<StatusVisitModelRequest> StatusVisitNursingList { get; set; } = new();

        public List<StatusEmergencyConsultationModelRequest> StatusEmergencyConsultationList { get; set; } = new();

        public List<CommonForm1ModelRequest> HospitalizationStatusList { get; set; } = new();

        public List<StatusShortTermAdmissionModelRequest> StatusShortTermAdmissionList { get; set; } = new();

        public List<CommonForm1ModelRequest> StatusHomeVisitList { get; set; } = new();
    }
}

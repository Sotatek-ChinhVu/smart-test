using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetPatientCommentResponse
    {
        public GetPatientCommentResponse(List<PatientInforModel> patientInforModels)
        {
            PatientInforModels = patientInforModels;
        }

        public List<PatientInforModel> PatientInforModels { get; set; }
    }
}

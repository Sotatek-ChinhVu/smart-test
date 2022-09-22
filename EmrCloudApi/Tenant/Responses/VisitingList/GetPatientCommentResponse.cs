using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetPatientCommentResponse
    {
        public GetPatientCommentResponse(PatientInforModel patientInforModels)
        {
            PatientInforModels = patientInforModels;
        }

        public PatientInforModel PatientInforModels { get; set; }
    }
}

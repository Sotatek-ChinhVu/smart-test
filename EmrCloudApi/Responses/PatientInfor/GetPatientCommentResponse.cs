using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class GetPatientCommentResponse
    {
        public GetPatientCommentResponse(PatientInforModel patientInforModels)
        {
            PatientInforModels = patientInforModels;
        }

        public PatientInforModel PatientInforModels { get; private set; }
    }
}

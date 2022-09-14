using Domain.Models.PatientComment;

namespace EmrCloudApi.Tenant.Responses.VisitingList
{
    public class GetPatientCommentResponse
    {
        public GetPatientCommentResponse(List<PatientCommentModel> patientCommentModels)
        {
            PatientCommentModels = patientCommentModels;
        }

        public List<PatientCommentModel> PatientCommentModels { get; set; }
    }
}

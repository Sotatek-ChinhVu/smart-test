using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class SearchEmptyIdResponse
    {
        public SearchEmptyIdResponse(List<PatientInforModel> patientInforModels)
        {
            PatientInforModels = patientInforModels;
        }

        public List<PatientInforModel> PatientInforModels { get; private set; }
    }
}

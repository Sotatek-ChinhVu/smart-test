using Domain.Models.SpecialNote.PatientInfo;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class SearchEmptyIdResponse
    {
        public SearchEmptyIdResponse(List<PatientInfoModel> patientInfoModels)
        {
            PatientInfoModels = patientInfoModels;
        }

        public List<PatientInfoModel> PatientInfoModels { get; private set; }
    }
}

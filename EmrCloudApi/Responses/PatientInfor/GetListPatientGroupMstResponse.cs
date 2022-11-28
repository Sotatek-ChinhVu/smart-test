using Domain.Models.PatientGroupMst;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class GetListPatientGroupMstResponse
    {
        public List<PatientGroupMstModel> PatientGroupMstModels { get; private set; }

        public GetListPatientGroupMstResponse(List<PatientGroupMstModel> patientGroupMstModels)
        {
            PatientGroupMstModels = patientGroupMstModels;
        }
    }
}

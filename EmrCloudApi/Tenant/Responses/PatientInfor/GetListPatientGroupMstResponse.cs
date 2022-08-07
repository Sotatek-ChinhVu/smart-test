using Domain.Models.PatientGroupMst;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
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

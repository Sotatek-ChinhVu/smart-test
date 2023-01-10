using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class GetListPatientInfoResponse
    {
        public GetListPatientInfoResponse(List<PatientInforModel> patientInfoList)
        {
            PatientInfoList = patientInfoList;
        }

        public List<PatientInforModel> PatientInfoList { get; private set; } = new List<PatientInforModel>();
    }
}

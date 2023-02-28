using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetListPatient;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class GetListPatientInfoResponse
    {
        public GetListPatientInfoResponse(List<GetListPatientInfoInputItem> patientInfoList)
        {
            PatientInfoList = patientInfoList;
        }

        public List<GetListPatientInfoInputItem> PatientInfoList { get; private set; } = new List<GetListPatientInfoInputItem>();
    }
}

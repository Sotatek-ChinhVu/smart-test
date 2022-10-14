using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public SavePatientInfoRequest(SavePatientInfoModel ptInformation)
        {
            PtInformation = ptInformation;
        }

        public SavePatientInfoModel PtInformation { get; set; }
    }
}
using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.PatientInformaiton
{
    public class GetAllPatientInforResponse
    {
        public List<PatientInfor> ListData { get; set; } = new List<PatientInfor>();
    }
}

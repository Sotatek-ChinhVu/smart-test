using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Responses.PatientInformaiton
{
    public class SearchPatientInforSimpleResponse
    {
        public List<PatientInforModel> Data { get; private set; }

        public SearchPatientInforSimpleResponse(List<PatientInforModel> data)
        {
            Data = data;
        }
    }
}
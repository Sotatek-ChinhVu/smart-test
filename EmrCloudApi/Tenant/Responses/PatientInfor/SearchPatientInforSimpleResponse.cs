using UseCase.PatientInfor.SearchSimple;

namespace EmrCloudApi.Tenant.Responses.PatientInformaiton
{
    public class SearchPatientInforSimpleResponse
    {
        public List<PatientInfoWithGroup> Data { get; private set; }

        public SearchPatientInforSimpleResponse(List<PatientInfoWithGroup> data)
        {
            Data = data;
        }
    }
}
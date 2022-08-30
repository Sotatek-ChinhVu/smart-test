using Domain.Models.PatientRaiinKubun;

namespace EmrCloudApi.Tenant.Responses.PatientRaiinKubun
{
    public class GetPatientRaiinKubunResponse
    {
        public List<PatientRaiinKubunModel> Data { get; private set; }

        public GetPatientRaiinKubunResponse(List<PatientRaiinKubunModel> data)
        {
            Data = data;
        }
    }
}

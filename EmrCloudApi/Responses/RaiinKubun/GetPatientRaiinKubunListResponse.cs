using UseCase.RaiinKbn.GetPatientRaiinKubunList;

namespace EmrCloudApi.Responses.RaiinKubun
{
    public class GetPatientRaiinKubunListResponse
    {
        public List<GetPatientRaiinKubunDto> Data { get; private set; }


        public GetPatientRaiinKubunListResponse(List<GetPatientRaiinKubunDto> data)
        {
            Data = data;
        }
    }
}

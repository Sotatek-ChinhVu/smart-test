using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKbn.GetPatientRaiinKubunList
{
    public class GetPatientRaiinKubunListOutputData : IOutputData
    {
        public GetPatientRaiinKubunListOutputData(List<GetPatientRaiinKubunDto> listPatientRaiinKubun, GetPatientRaiinKubunListStatus status)
        {
            ListPatientRaiinKubun = listPatientRaiinKubun;
            Status = status;
        }

        public List<GetPatientRaiinKubunDto> ListPatientRaiinKubun { get; private set; }

        public GetPatientRaiinKubunListStatus Status { get; private set; }

    }
}

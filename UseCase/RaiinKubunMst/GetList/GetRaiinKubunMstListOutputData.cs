using Domain.Models.RaiinKubunMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.GetList
{
    public class GetRaiinKubunMstListOutputData : IOutputData
    {
        public List<RaiinKubunMstModel> RaiinKubunList { get; private set; }

        public GetRaiinKubunMstListOutputData(List<RaiinKubunMstModel> raiinKubunList)
        {
            RaiinKubunList = raiinKubunList;
        }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.GetList
{
    public class GetRaiinKubunMstListInputData : IInputData<GetRaiinKubunMstListOutputData>
    {
        public bool IsDeleted { get; private set; }

        public GetRaiinKubunMstListInputData(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}

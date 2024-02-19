using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.GetList
{
    public class GetRaiinKubunMstListInputData : IInputData<GetRaiinKubunMstListOutputData>
    {
        public int HpId { get; private set; }
        public bool IsDeleted { get; private set; }

        public GetRaiinKubunMstListInputData(int hpId, bool isDeleted)
        {
            HpId = hpId;
            IsDeleted = isDeleted;
        }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.User.GetListJobMst
{
    public class GetListJobMstInputData : IInputData<GetListJobMstOutputData>
    {
        public GetListJobMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}

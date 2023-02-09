using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.GetGroupNameMst
{
    public class GetGroupNameMstInputData : IInputData<GetGroupNameMstOutputData>
    {
        public GetGroupNameMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}

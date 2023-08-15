using UseCase.Core.Sync.Core;

namespace UseCase.PtGroupMst.GetGroupNameMst
{
    public class GetGroupNameMstInputData : IInputData<GetGroupNameMstOutputData>
    {
        public GetGroupNameMstInputData(int hpId, bool isGetAll)
        {
            HpId = hpId;
            IsGetAll = isGetAll;
        }

        public int HpId { get; private set; }

        public bool IsGetAll { get; private set; }
    }
}

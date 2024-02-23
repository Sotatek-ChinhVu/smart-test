using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CheckJihiSbtExistsInTenMst
{
    public sealed class CheckJihiSbtExistsInTenMstInputData : IInputData<CheckJihiSbtExistsInTenMstOutputData>
    {
        public CheckJihiSbtExistsInTenMstInputData(int hpId, int jihiSbt)
        {
            HpId = hpId;
            JihiSbt = jihiSbt;
        }
        public int HpId { get; private set; }
        public int JihiSbt { get; private set; }
    }
}

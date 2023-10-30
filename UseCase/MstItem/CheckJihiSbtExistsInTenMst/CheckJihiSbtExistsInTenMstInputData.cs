using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CheckJihiSbtExistsInTenMst
{
    public sealed class CheckJihiSbtExistsInTenMstInputData : IInputData<CheckJihiSbtExistsInTenMstOutputData>
    {
        public CheckJihiSbtExistsInTenMstInputData(int jihiSbt)
        {
            JihiSbt = jihiSbt;
        }
        public int JihiSbt {  get; private set; }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CheckJihiSbtExistsInTenMst
{
    public sealed class CheckJihiSbtExistsInTenMstOutputData : IOutputData
    {
        public CheckJihiSbtExistsInTenMstOutputData(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}

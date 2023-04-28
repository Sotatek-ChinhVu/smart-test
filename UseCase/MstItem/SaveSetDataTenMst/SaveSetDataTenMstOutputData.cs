using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveSetDataTenMst
{
    public class SaveSetDataTenMstOutputData : IOutputData
    {
        public SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus status)
        {
            Status = status;
        }

        public SaveSetDataTenMstStatus Status { get; private set; }
    }
}

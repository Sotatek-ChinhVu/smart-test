using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateKensaStdMst
{
    public class UpdateKensaStdMstOutputData : IOutputData
    {
        public UpdateKensaStdMstOutputData(UpdateKensaStdMstStatus status)
        {
            Status = status;
        }

        public UpdateKensaStdMstStatus Status { get; private set; }
    }
}

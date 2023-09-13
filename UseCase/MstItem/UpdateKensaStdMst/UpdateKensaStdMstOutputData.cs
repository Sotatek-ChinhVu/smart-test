using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateKensaStdMst
{
    public class UpdateKensaStdMstOutputData : IOutputData
    {
        public UpdateKensaStdMstOutputData(UpdateKensaStdMstStatus status)
        {

        }

        public UpdateKensaStdMstStatus status { get; private set; }
    }
}

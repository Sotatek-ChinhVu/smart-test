using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ExistUsedKensaItemCd
{
    public class ExistUsedKensaItemCdOutputData : IOutputData
    {
        public ExistUsedKensaItemCdOutputData(ExistUsedKensaItemCdStatus status)
        {
            Status = status;
        }

        public ExistUsedKensaItemCdStatus Status { get; private set; }
    }
}

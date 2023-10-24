using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.IsKensaItemOrdering
{
    public class IsKensaItemOrderingOutputData : IOutputData
    {
        public IsKensaItemOrderingOutputData(IsKensaItemOrderingStatus status)
        {
            Status = status;
        }

        public IsKensaItemOrderingStatus Status { get; private set; }
    }
}

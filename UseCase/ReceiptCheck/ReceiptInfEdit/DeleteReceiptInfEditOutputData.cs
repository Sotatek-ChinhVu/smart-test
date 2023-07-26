using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck.ReceiptInfEdit
{
    public class DeleteReceiptInfEditOutputData : IOutputData
    {
        public DeleteReceiptInfEditOutputData(DeleteReceiptInfStatus status)
        {
            Status = status;
        }

        public DeleteReceiptInfStatus Status { get; private set; }
    }
}

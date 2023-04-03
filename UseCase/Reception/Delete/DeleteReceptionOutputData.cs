using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Delete
{
    public class DeleteReceptionOutputData : IOutputData
    {
        public DeleteReceptionOutputData(DeleteReceptionStatus status)
        {
            Status = status;
        }

        public DeleteReceptionStatus Status { get; private set; }
    }
}

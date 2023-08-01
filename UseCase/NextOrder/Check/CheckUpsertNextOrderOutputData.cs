using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Check
{
    public class CheckUpsertNextOrderOutputData : IOutputData
    {
        public CheckUpsertNextOrderOutputData(CheckUpsertNextOrderStatus status)
        {
            Status = status;
        }

        public CheckUpsertNextOrderStatus Status { get; set; }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.AutoCheckOrder
{
    public class AutoCheckOrderOutputData : IOutputData
    {
        public AutoCheckOrderOutputData(AutoCheckOrderStatus status, List<AutoCheckOrderItem> autoCheckOrderItems)
        {
            Status = status;
            AutoCheckOrderItems = autoCheckOrderItems;
        }

        public AutoCheckOrderStatus Status { get; private set; }
        public List<AutoCheckOrderItem> AutoCheckOrderItems { get; private set; }
    }
}

using UseCase.Core.Sync.Core;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.ChangeAfterAutoCheckOrder
{
    public class ChangeAfterAutoCheckOrderOutputData : IOutputData
    {
        public ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus status, List<ChangeAfterAutoCheckOrderItem> odrInfItems)
        {
            Status = status;
            OdrInfItems = odrInfItems;
        }

        public ChangeAfterAutoCheckOrderStatus Status { get; private set; }
        public List<ChangeAfterAutoCheckOrderItem> OdrInfItems { get; private set; }
    }
}

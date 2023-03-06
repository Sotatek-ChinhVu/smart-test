using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.ChangeAfterAutoCheckOrder
{
    public class ChangeAfterAutoCheckOrderOutputData : IOutputData
    {
        public ChangeAfterAutoCheckOrderOutputData(ChangeAfterAutoCheckOrderStatus status, List<OdrInfItemInputData> odrInfItems)
        {
            Status = status;
            OdrInfItems = odrInfItems;
        }

        public ChangeAfterAutoCheckOrderStatus Status { get; private set; }
        public List<OdrInfItemInputData> OdrInfItems { get; private set; }
    }
}

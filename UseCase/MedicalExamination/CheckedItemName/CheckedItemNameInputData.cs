using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.CheckedItemName
{
    public class CheckedItemNameInputData : IInputData<CheckedItemNameOutputData>
    {
        public CheckedItemNameInputData(List<OdrInfItemInputData> odrInfs)
        {
            OdrInfs = odrInfs;
        }

        public List<OdrInfItemInputData> OdrInfs { get; private set; }
    }
}

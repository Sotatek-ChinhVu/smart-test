using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredInputData : IInputData<CheckedExpiredOutputData>
    {
        public CheckedExpiredInputData(int hpId, int sinDate, List<CheckedExpiredItem> checkedExpiredItems)
        {
            HpId = hpId;
            SinDate = sinDate;
            CheckedExpiredItems = checkedExpiredItems;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public List<CheckedExpiredItem> CheckedExpiredItems { get; private set; }
    }
}

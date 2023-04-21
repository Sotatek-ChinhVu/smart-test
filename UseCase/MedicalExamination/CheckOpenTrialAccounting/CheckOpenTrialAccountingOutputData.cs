using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace UseCase.MedicalExamination.CheckOpenTrialAccounting
{
    public class CheckOpenTrialAccountingOutputData : IOutputData
    {
        public CheckOpenTrialAccountingOutputData(bool isHokenPatternSelect, List<GairaiRihaItem> gairaiRihaItems, double systemSetting, bool isExistYoboItemOnly, CheckOpenTrialAccountingStatus status)
        {
            IsHokenPatternSelect = isHokenPatternSelect;
            GairaiRihaItems = gairaiRihaItems;
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
            Status = status;
        }

        public bool IsHokenPatternSelect { get; private set; }
        public List<GairaiRihaItem> GairaiRihaItems { get; private set; }
        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
        public CheckOpenTrialAccountingStatus Status { get; private set; }
    }
}

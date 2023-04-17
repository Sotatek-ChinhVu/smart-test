using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckOpenTrialAccounting
{
    public class CheckOpenTrialAccountingOutputData : IOutputData
    {
        public CheckOpenTrialAccountingOutputData(bool isHokenPatternSelect, int type, string itemName, int lastDaySanteiRiha, string rihaItemName, double systemSetting, bool isExistYoboItemOnly, CheckOpenTrialAccountingStatus status)
        {
            IsHokenPatternSelect = isHokenPatternSelect;
            Type = type;
            ItemName = itemName;
            LastDaySanteiRiha = lastDaySanteiRiha;
            RihaItemName = rihaItemName;
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
            Status = status;
        }

        public bool IsHokenPatternSelect { get; private set; }
        public int Type { get; private set; }
        public string ItemName { get; private set; }
        public int LastDaySanteiRiha { get; private set; }
        public string RihaItemName { get; private set; }
        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
        public CheckOpenTrialAccountingStatus Status { get; private set; }
    }
}

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckOpenTrialAccountingResponse
    {
        public CheckOpenTrialAccountingResponse(int type, string itemName, int lastDaySanteiRiha, string rihaItemName, double systemSetting, bool isExistYoboItemOnly)
        {
            Type = type;
            ItemName = itemName;
            LastDaySanteiRiha = lastDaySanteiRiha;
            RihaItemName = rihaItemName;
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
        }

        public int Type { get; private set; }
        public string ItemName { get; private set; }
        public int LastDaySanteiRiha { get; private set; }
        public string RihaItemName { get; private set; }
        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
    }
}

using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckOpenTrialAccountingResponse
    {
        public CheckOpenTrialAccountingResponse(bool isHokenPatternSelect, List<GairaiRihaItem> gairaiRihaItems, double systemSetting, bool isExistYoboItemOnly)
        {
            IsHokenPatternSelect = isHokenPatternSelect;
            GairaiRihaItems = gairaiRihaItems;
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
        }

        public bool IsHokenPatternSelect { get; private set; }
        public List<GairaiRihaItem> GairaiRihaItems { get; private set; }
        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
    }
}

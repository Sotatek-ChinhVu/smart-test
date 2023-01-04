using UseCase.MedicalExamination.GetValidJihiYobo;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetValidJihiYoboResponse
    {
        public GetValidJihiYoboResponse(double systemSetting, bool isExistYoboItemOnly)
        {
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
        }

        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
    }
}
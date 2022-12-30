using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetValidJihiYobo
{
    public class GetValidJihiYoboOutputData : IOutputData
    {
        public GetValidJihiYoboOutputData(double systemSetting, bool isExistYoboItemOnly, GetValidJihiYoboStatus status)
        {
            SystemSetting = systemSetting;
            IsExistYoboItemOnly = isExistYoboItemOnly;
            Status = status;
        }

        public double SystemSetting { get; private set; }
        public bool IsExistYoboItemOnly { get; private set; }
        public GetValidJihiYoboStatus Status { get; private set; }
    }
}

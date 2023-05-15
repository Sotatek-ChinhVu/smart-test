using UseCase.Core.Sync.Core;

namespace UseCase.TimeZoneConf.SaveTimeZoneConf
{
    public class SaveTimeZoneConfOutputData : IOutputData
    {
        public SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus status)
        {
            Status = status;
        }

        public SaveTimeZoneConfStatus Status { get; private set; }
    }
}

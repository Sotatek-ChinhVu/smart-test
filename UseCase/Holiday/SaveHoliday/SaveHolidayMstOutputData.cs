using UseCase.Core.Sync.Core;

namespace UseCase.Holiday.SaveHoliday
{
    public class SaveHolidayMstOutputData : IOutputData
    {
        public SaveHolidayMstOutputData(SaveHolidayMstStatus status)
        {
            Status = status;
        }

        public SaveHolidayMstStatus Status { get; private set; }
    }
}

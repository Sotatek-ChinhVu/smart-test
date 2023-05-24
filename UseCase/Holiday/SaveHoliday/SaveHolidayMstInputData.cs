using Domain.Models.FlowSheet;
using UseCase.Core.Sync.Core;

namespace UseCase.Holiday.SaveHoliday
{
    public class SaveHolidayMstInputData : IInputData<SaveHolidayMstOutputData>
    {
        public SaveHolidayMstInputData(HolidayModel holiday, int userId)
        {
            Holiday = holiday;
            UserId = userId;
        }

        public HolidayModel Holiday { get; private set; }

        public int UserId { get; private set; }
    }
}

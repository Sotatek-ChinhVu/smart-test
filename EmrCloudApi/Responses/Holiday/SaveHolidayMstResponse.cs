using UseCase.Holiday.SaveHoliday;

namespace EmrCloudApi.Responses.Holiday
{
    public class SaveHolidayMstResponse
    {
        public SaveHolidayMstResponse(SaveHolidayMstStatus status)
        {
            Status = status;
        }

        public SaveHolidayMstStatus Status { get; private set; }
    }
}

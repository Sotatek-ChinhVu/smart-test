namespace EmrCloudApi.Requests.Holiday
{
    public class SaveHolidayMstRequest
    {
        public SaveHolidayMstRequest(HolidayDto holiday)
        {
            Holiday = holiday;
        }

        public HolidayDto Holiday { get; private set; }
    }
}

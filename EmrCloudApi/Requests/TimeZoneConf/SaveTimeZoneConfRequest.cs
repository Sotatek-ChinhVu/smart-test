namespace EmrCloudApi.Requests.TimeZoneConf
{
    public class SaveTimeZoneConfRequest
    {
        public SaveTimeZoneConfRequest(List<TimeZoneConfDto> timeZoneConfs)
        {
            TimeZoneConfs = timeZoneConfs;
        }

        public List<TimeZoneConfDto> TimeZoneConfs { get; private set; }
    }
}

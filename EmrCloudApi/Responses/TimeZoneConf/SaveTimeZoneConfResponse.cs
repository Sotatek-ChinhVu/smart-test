using UseCase.TimeZoneConf.SaveTimeZoneConf;

namespace EmrCloudApi.Responses.TimeZoneConf
{
    public class SaveTimeZoneConfResponse
    {
        public SaveTimeZoneConfResponse(SaveTimeZoneConfStatus status)
        {
            Status = status;
        }

        public SaveTimeZoneConfStatus Status { get; private set; }
    }
}

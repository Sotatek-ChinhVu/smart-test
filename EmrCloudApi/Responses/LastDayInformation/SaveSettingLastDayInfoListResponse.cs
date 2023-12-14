namespace EmrCloudApi.Responses.LastDayInformation
{
    public class SaveSettingLastDayInfoListResponse
    {
        public SaveSettingLastDayInfoListResponse(bool successed)
        {
            Successed = successed;
        }

        public bool Successed { get; set; }
    }
}

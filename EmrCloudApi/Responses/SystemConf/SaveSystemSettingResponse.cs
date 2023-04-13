namespace EmrCloudApi.Responses.SystemConf
{
    public class SaveSystemSettingResponse
    {
        public SaveSystemSettingResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}

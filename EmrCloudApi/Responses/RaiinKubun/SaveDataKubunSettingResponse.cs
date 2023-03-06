namespace EmrCloudApi.Responses.RaiinKubun
{
    public class SaveDataKubunSettingResponse
    {
        public SaveDataKubunSettingResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}

namespace EmrCloudApi.Requests.RaiinListSetting
{
    public class SaveRaiinListSettingRequest
    {
        public SaveRaiinListSettingRequest(List<RaiinListMstDto> raiinListSettings)
        {
            RaiinListSettings = raiinListSettings;
        }

        public List<RaiinListMstDto> RaiinListSettings { get; private set; }
    }
}

using UseCase.RaiinListSetting.SaveRaiinListSetting;

namespace EmrCloudApi.Responses.RaiinListSetting
{
    public class SaveRaiinListSettingResponse
    {
        public SaveRaiinListSettingResponse(SaveRaiinListSettingStatus status)
        {
            Status = status;
        }

        public SaveRaiinListSettingStatus Status { get; private set; }
    }
}

using Domain.Models.VisitingListSetting;

namespace EmrCloudApi.Requests.VisitingList;

public class SaveVisitingListSettingsRequest
{
    public VisitingListSettingModel Settings { get; set; } = null!;
}

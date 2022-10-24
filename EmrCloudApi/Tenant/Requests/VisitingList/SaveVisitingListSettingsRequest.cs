using Domain.Models.VisitingListSetting;

namespace EmrCloudApi.Tenant.Requests.VisitingList;

public class SaveVisitingListSettingsRequest
{
    public VisitingListSettingModel Settings { get; set; } = null!;
}

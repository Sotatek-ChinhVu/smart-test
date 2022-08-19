using Domain.Models.VisitingListSetting;

namespace EmrCloudApi.Tenant.Requests.VisitingList;

public class SaveVisitingListSettingsRequest
{
    public int UserId { get; set; }
    public VisitingListSettingModel Settings { get; set; } = null!;
}

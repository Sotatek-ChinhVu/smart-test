using Domain.Models.Reception;
using Domain.Models.VisitingListSetting;

namespace EmrCloudApi.Responses.Reception;

public class GetReceptionSettingsResponse
{
    public GetReceptionSettingsResponse(VisitingListSettingModel settings)
    {
        Settings = settings;
    }

    public VisitingListSettingModel Settings { get; private set; }
}

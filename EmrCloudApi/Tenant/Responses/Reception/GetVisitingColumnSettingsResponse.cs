using Domain.Models.ColumnSetting;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetVisitingColumnSettingsResponse
{
    public GetVisitingColumnSettingsResponse(List<ColumnSettingModel> settings)
    {
        Settings = settings;
    }

    public List<ColumnSettingModel> Settings { get; private set; }
}

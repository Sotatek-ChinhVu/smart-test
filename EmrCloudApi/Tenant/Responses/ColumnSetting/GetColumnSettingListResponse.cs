using Domain.Models.ColumnSetting;

namespace EmrCloudApi.Tenant.Responses.ColumnSetting;

public class GetColumnSettingListResponse
{
    public GetColumnSettingListResponse(List<ColumnSettingModel> settings)
    {
        Settings = settings;
    }

    public List<ColumnSettingModel> Settings { get; private set; }
}

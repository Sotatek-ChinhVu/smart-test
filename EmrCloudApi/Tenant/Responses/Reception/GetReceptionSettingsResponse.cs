using Domain.Models.Reception;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetReceptionSettingsResponse
{
    public GetReceptionSettingsResponse(ReceptionSettings settings)
    {
        Settings = settings;
    }

    public ReceptionSettings Settings { get; private set; }
}

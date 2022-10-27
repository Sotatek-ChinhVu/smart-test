using Domain.Models.TimeZone;

namespace EmrCloudApi.Tenant.Responses.Reception;

public class GetDefaultSelectedTimeResponse
{
    public GetDefaultSelectedTimeResponse(DefaultSelectedTimeModel data)
    {
        Data = data;
    }

    public DefaultSelectedTimeModel Data { get; private set; }
}

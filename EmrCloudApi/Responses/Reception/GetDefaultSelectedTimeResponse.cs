using UseCase.Reception.GetDefaultSelectedTime;

namespace EmrCloudApi.Responses.Reception;

public class GetDefaultSelectedTimeResponse
{
    public GetDefaultSelectedTimeResponse(DefaultSelectedTimeModel data)
    {
        Data = data;
    }

    public DefaultSelectedTimeModel Data { get; private set; }
}

using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class GetLastKaruteResponse
{
    public GetLastKaruteResponse(ReceptionModel receptionModels)
    {
        ReceptionModels = receptionModels;
    }

    public ReceptionModel ReceptionModels { get; private set; }
}

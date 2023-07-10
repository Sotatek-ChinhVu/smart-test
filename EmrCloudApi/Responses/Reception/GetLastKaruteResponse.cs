using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Reception;

public class GetLastKaruteResponse
{
    public GetLastKaruteResponse(ReceptionModel receptionModel)
    {
        ReceptionModel = receptionModel;
    }

    public ReceptionModel ReceptionModel { get; private set; }
}

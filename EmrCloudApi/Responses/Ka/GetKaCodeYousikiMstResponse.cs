using Domain.Models.Ka;

namespace EmrCloudApi.Responses.Ka;

public class GetKaCodeYousikiMstResponse
{
    public GetKaCodeYousikiMstResponse(List<KacodeYousikiMstModel> kacodeYousikiMstModels)
    {
        KacodeYousikiMstModels = kacodeYousikiMstModels;
    }

    public List<KacodeYousikiMstModel> KacodeYousikiMstModels { get; private set; }
}

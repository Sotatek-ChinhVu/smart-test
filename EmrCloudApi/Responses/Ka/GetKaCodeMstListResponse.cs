using Domain.Models.Ka;

namespace EmrCloudApi.Responses.Ka;

public class GetKaCodeMstListResponse
{
    public GetKaCodeMstListResponse(List<KaCodeMstModel> kaCodeMstModels)
    {
        KaCodeMstModels = kaCodeMstModels;
    }

    public List<KaCodeMstModel> KaCodeMstModels { get; private set; }
}

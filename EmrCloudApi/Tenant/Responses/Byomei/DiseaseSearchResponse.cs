using Domain.Models.Byomei;

namespace EmrCloudApi.Tenant.Responses.Byomei;

public class DiseaseSearchResponse
{
    public DiseaseSearchResponse(List<ByomeiMstModel> data)
    {
        Data = data;
    }

    public List<ByomeiMstModel> Data { get; private set; }
}

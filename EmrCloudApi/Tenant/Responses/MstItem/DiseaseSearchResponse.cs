using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem;

public class DiseaseSearchResponse
{
    public DiseaseSearchResponse(List<ByomeiMstModel> data)
    {
        Data = data;
    }

    public List<ByomeiMstModel> Data { get; private set; }
}

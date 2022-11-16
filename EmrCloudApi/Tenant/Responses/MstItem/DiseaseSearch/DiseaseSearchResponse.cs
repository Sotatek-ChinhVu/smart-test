namespace EmrCloudApi.Tenant.Responses.MstItem.DiseaseSearch;

public class DiseaseSearchResponse
{
    public DiseaseSearchResponse(List<DiseaseSearchModel> data)
    {
        Data = data;
    }

    public List<DiseaseSearchModel> Data { get; private set; }
}

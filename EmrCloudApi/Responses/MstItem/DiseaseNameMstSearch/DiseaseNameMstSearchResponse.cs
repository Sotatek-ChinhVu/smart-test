namespace EmrCloudApi.Responses.MstItem.DiseaseNameMstSearch;

public class DiseaseNameMstSearchResponse
{
    public DiseaseNameMstSearchResponse(List<DiseaseNameMstSearchModel> data)
    {
        Data = data;
    }

    public List<DiseaseNameMstSearchModel> Data { get; private set; }
}

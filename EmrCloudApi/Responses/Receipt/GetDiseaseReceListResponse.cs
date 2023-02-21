using UseCase.Receipt.GetDiseaseReceList;

namespace EmrCloudApi.Responses.Receipt;

public class GetDiseaseReceListResponse
{
    public GetDiseaseReceListResponse(List<DiseaseReceOutputItem> diseaseReceList)
    {
        DiseaseReceList = diseaseReceList;
    }

    public List<DiseaseReceOutputItem> DiseaseReceList { get; private set; }
}

using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetDiseaseReceList;

public class GetDiseaseReceListOutputData : IOutputData
{
    public GetDiseaseReceListOutputData(List<DiseaseReceOutputItem> diseaseReceList, GetDiseaseReceListStatus status)
    {
        DiseaseReceList = diseaseReceList;
        Status = status;
    }

    public List<DiseaseReceOutputItem> DiseaseReceList { get; private set; }

    public GetDiseaseReceListStatus Status { get; private set; }
}

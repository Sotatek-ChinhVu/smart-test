using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetAllByomeiByPtId;

public class GetAllByomeiByPtIdOutputData : IOutputData
{
    public List<PtDiseaseModel> DiseaseList { get; private set; }

    public GetAllByomeiByPtIdStatus Status { get; private set; }

    public GetAllByomeiByPtIdOutputData(List<PtDiseaseModel> diseaseList, GetAllByomeiByPtIdStatus status)
    {
        DiseaseList = diseaseList;
        Status = status;
    }
}

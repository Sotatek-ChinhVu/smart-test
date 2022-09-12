using Domain.Models.Byomei;
using UseCase.Core.Sync.Core;

namespace UseCase.Byomei.DiseaseSearch;

public class DiseaseSearchOutputData : IOutputData
{
    public List<ByomeiMstModel> ListData { get; private set; }

    public DiseaseSearchStatus Status { get; private set; }

    public DiseaseSearchOutputData(DiseaseSearchStatus status)
    {
        ListData = new List<ByomeiMstModel>();
        Status = status;
    }
    public DiseaseSearchOutputData(List<ByomeiMstModel> listData, DiseaseSearchStatus status)
    {
        ListData = listData;
        Status = status;
    }
}

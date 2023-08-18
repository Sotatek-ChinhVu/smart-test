using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDiseaseList;

public class GetDiseaseListOutputData : IOutputData
{
    public List<ByomeiMstModel> ListData { get; private set; }

    public GetDiseaseListStatus Status { get; private set; }

    public GetDiseaseListOutputData(List<ByomeiMstModel> listData, GetDiseaseListStatus status)
    {
        ListData = listData;
        Status = status;
    }
}

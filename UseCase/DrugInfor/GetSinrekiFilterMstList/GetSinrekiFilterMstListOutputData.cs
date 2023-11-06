using Domain.Models.DrugInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetSinrekiFilterMstList;

public class GetSinrekiFilterMstListOutputData : IOutputData
{
    public GetSinrekiFilterMstListOutputData(List<SinrekiFilterMstModel> sinrekiFilterMstList, GetSinrekiFilterMstListStatus status)
    {
        SinrekiFilterMstList = sinrekiFilterMstList;
        Status = status;
    }

    public List<SinrekiFilterMstModel> SinrekiFilterMstList { get; private set; }

    public GetSinrekiFilterMstListStatus Status { get; private set; }
}

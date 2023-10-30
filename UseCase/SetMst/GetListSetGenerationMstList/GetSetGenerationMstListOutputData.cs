using Domain.Models.SetGenerationMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.GetListSetGenerationMst;

public class GetSetGenerationMstListOutputData : IOutputData
{
    public GetSetGenerationMstListOutputData(GetSetGenerationMstListStatus status, List<SetGenerationMstModel> setGenerationList)
    {
        Status = status;
        SetGenerationList = setGenerationList;
    }

    public GetSetGenerationMstListStatus Status { get; private set; }

    public List<SetGenerationMstModel> SetGenerationList { get; private set; }
}

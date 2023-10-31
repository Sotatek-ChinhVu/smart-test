using Domain.Models.SetKbnMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;

public class GetSetKbnMstListByGenerationIdOutputData : IOutputData
{
    public GetSetKbnMstListByGenerationIdOutputData(List<SetKbnMstModel> setKbnMstList, GetSetKbnMstListByGenerationIdStatus status)
    {
        SetKbnMstList = setKbnMstList;
        Status = status;
    }

    public List<SetKbnMstModel> SetKbnMstList { get; private set; }

    public GetSetKbnMstListByGenerationIdStatus Status { get; private set; }
}

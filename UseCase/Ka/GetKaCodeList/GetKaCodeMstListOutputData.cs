using Domain.Models.Ka;
using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetKaCodeList;

public class GetKaCodeMstListOutputData : IOutputData
{
    public GetKaCodeMstListOutputData(GetKaCodeMstListStatus status, List<KaCodeMstModel> kaCodeMstModels)
    {
        Status = status;
        KaCodeMstModels = kaCodeMstModels;
    }

    public GetKaCodeMstListStatus Status { get; private set; }
    public List<KaCodeMstModel> KaCodeMstModels { get; private set; }
}

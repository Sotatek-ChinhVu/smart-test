using Domain.Models.Ka;
using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetKacodeMstYossi;

public class GetKacodeMstYossiOutputData : IOutputData
{
    public GetKacodeMstYossiOutputData(GetKacodeMstYossiStatus status, List<KaCodeMstModel> kaCodeMstModels)
    {
        Status = status;
        KaCodeMstModels = kaCodeMstModels;
    }

    public GetKacodeMstYossiStatus Status { get; private set; }
    public List<KaCodeMstModel> KaCodeMstModels { get; private set; }
}

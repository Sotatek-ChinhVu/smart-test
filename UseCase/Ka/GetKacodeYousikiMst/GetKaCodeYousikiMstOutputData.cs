using Domain.Models.Ka;
using UseCase.Core.Sync.Core;
using UseCase.Ka.GetKacodeMstYossi;

namespace UseCase.Ka.GetKacodeYousikiMst;

public class GetKaCodeYousikiMstOutputData : IOutputData
{
    public GetKaCodeYousikiMstOutputData(GetKaCodeYousikiMstStatus status, List<KacodeYousikiMstModel> kaCodeMstModels)
    {
        Status = status;
        KaCodeMstModels = kaCodeMstModels;
    }

    public GetKaCodeYousikiMstStatus Status { get; private set; }
    public List<KacodeYousikiMstModel> KaCodeMstModels { get; private set; }
}

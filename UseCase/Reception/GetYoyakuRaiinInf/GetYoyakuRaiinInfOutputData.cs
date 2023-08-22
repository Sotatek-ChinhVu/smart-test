using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetYoyakuRaiinInf;

public class GetYoyakuRaiinInfOutputData : IOutputData
{
    public GetYoyakuRaiinInfOutputData(ReceptionModel raiinInf, GetYoyakuRaiinInfStatus status)
    {
        RaiinInf = raiinInf;
        Status = status;
    }

    public ReceptionModel RaiinInf { get; private set; }

    public GetYoyakuRaiinInfStatus Status { get; private set; }
}

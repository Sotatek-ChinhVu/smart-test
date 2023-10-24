using Domain.Models.HpInf;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetHpInf;

public class GetHpInfOutputData : IOutputData
{
    public GetHpInfOutputData(HpInfModel hpInf, GetHpInfStatus status)
    {
        HpInf = hpInf;
        Status = status;
    }

    public HpInfModel HpInf { get; private set; }

    public GetHpInfStatus Status { get; private set; }
}

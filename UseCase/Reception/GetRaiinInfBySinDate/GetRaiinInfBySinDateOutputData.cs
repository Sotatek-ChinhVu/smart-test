using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetRaiinInfBySinDate;

public class GetRaiinInfBySinDateOutputData : IOutputData
{
    public GetRaiinInfBySinDateOutputData(ReceptionModel raiinInf, GetRaiinInfBySinDateStatus status)
    {
        RaiinInf = raiinInf;
        Status = status;
    }

    public ReceptionModel RaiinInf { get; private set; }

    public GetRaiinInfBySinDateStatus Status { get; private set; }
}

using UseCase.Core.Sync.Core;
using UseCase.Reception;
using UseCase.Reception.GetListRaiinInfs;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<ReceptionGetDto> raiinInfs, GetListRaiinInfStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfStatus Status { get; private set; }
    public List<ReceptionGetDto> RaiinInfs { get; private set; }
}
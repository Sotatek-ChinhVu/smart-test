using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineInRaiinInf;

public class UpdateOnlineInRaiinInfOutputData : IOutputData
{
    public UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus status)
    {
        Status = status;
    }

    public UpdateOnlineInRaiinInfStatus Status { get; private set; }
}

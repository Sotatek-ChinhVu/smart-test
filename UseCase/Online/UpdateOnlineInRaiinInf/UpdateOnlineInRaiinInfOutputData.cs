using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineInRaiinInf;

public class UpdateOnlineInRaiinInfOutputData : IOutputData
{
    public UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus status)
    {
        Status = status;
        ReceptionInfos = new();
    }

    public UpdateOnlineInRaiinInfOutputData(UpdateOnlineInRaiinInfStatus status, List<ReceptionRowModel> receptionInfos)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
    }

    public UpdateOnlineInRaiinInfStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }
}

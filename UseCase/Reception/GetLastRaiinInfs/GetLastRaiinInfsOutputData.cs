using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetLastRaiinInfs;

public class GetLastRaiinInfsOutputData : IOutputData
{
    public GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus status, List<ReceptionModel> listReceptions)
    {
        Status = status;
        ListReceptions = listReceptions;
    }

    public GetLastRaiinInfsOutputData(GetLastRaiinInfsStatus status)
    {
        Status = status;
        ListReceptions = new();
    }

    public GetLastRaiinInfsStatus Status { get; private set; }

    public List<ReceptionModel> ListReceptions { get; private set; }
}

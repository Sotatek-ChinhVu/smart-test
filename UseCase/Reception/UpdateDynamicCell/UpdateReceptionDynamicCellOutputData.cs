using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateDynamicCell;

public class UpdateReceptionDynamicCellOutputData : IOutputData
{
    public UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus status, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }

    public UpdateReceptionDynamicCellOutputData(UpdateReceptionDynamicCellStatus status)
    {
        Status = status;
        ReceptionInfos = new();
        SameVisitList = new();
    }

    public UpdateReceptionDynamicCellStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }
}

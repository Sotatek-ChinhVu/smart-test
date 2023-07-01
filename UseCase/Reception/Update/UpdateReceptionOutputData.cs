using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Update;

public class UpdateReceptionOutputData : IOutputData
{
    public UpdateReceptionOutputData(UpdateReceptionStatus status, List<ReceptionRowModel> receptionRowModels, List<SameVisitModel> sameVisitList)
    {
        Status = status;
        ReceptionInfos = receptionRowModels;
        SameVisitList = sameVisitList;
    }

    public UpdateReceptionStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }
}

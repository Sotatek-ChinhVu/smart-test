using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Update;

public class UpdateReceptionOutputData : IOutputData
{
    public UpdateReceptionOutputData(UpdateReceptionStatus status, List<ReceptionRowModel> receptionRowModels)
    {
        Status = status;
        ReceptionInfos = receptionRowModels;
    }

    public UpdateReceptionStatus Status { get; private set; }
    public List<ReceptionRowModel> ReceptionInfos { get; private set; }
}

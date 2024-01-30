using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOQConfirmation;

public class SaveOQConfirmationOutputData : IOutputData
{
    public SaveOQConfirmationOutputData(SaveOQConfirmationStatus status, List<ReceptionRowModel> receptionInfos)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
    }

    public SaveOQConfirmationOutputData(SaveOQConfirmationStatus status)
    {
        Status = status;
        ReceptionInfos = new();
    }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public SaveOQConfirmationStatus Status { get; private set; }
}

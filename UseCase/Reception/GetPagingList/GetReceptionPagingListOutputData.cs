using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetPagingList;

public class GetReceptionPagingListOutputData : IOutputData
{
    public GetReceptionPagingListOutputData(GetReceptionPagingListStatus status)
    {
        Status = status;
    }

    public GetReceptionPagingListOutputData(GetReceptionPagingListStatus status, List<ReceptionRowModel> receptionInfos, int totalItems)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
        TotalItems = totalItems;
    }

    public GetReceptionPagingListStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; } = new();

    public int TotalItems { get; private set; }
}

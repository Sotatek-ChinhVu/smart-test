using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListOutputData : IOutputData
{
    public GetReceptionListOutputData(GetReceptionListStatus status)
    {
        Status = status;
    }

    public GetReceptionListOutputData(GetReceptionListStatus status, List<ReceptionRowModel> receptionInfos)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
    }

    public GetReceptionListStatus Status { get; private set; }
    public List<ReceptionRowModel> ReceptionInfos { get; private set; } = new List<ReceptionRowModel>();
}

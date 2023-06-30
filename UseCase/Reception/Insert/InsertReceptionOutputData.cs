using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Insert;

public class InsertReceptionOutputData : IOutputData
{
    public InsertReceptionOutputData(InsertReceptionStatus status, long raiinNo, List<ReceptionRowModel> receptionInfos)
    {
        Status = status;
        RaiinNo = raiinNo;
        ReceptionInfos = receptionInfos;
    }

    public InsertReceptionStatus Status { get; private set; }
    public long RaiinNo { get; private set; }
    public List<ReceptionRowModel> ReceptionInfos { get; private set; }
}

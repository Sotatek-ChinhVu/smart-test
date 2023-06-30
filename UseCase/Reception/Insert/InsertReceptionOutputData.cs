using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Insert;

public class InsertReceptionOutputData : IOutputData
{
    public InsertReceptionOutputData(InsertReceptionStatus status, long raiinNo, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        Status = status;
        RaiinNo = raiinNo;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }

    public InsertReceptionStatus Status { get; private set; }

    public long RaiinNo { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }
}

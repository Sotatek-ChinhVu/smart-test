using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.SaveAccounting
{
    public class SaveAccountingOutputData : IOutputData
    {
        public SaveAccountingOutputData(SaveAccountingStatus status, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList, List<long> raiinNoPrint)
        {
            Status = status;
            ReceptionInfos = receptionInfos;
            SameVisitList = sameVisitList;
            RaiinNoPrint = raiinNoPrint;
        }

        public SaveAccountingStatus Status { get; private set; }

        public List<ReceptionRowModel> ReceptionInfos { get; private set; }

        public List<SameVisitModel> SameVisitList { get; private set; }

        public List<long> RaiinNoPrint { get; private set; }
    }
}

using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetHistory;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderOutputData : IOutputData
    {
        public GetAccountingHistoryOrderOutputData(List<HistoryKarteOdrRaiinItem> raiinfList, GetAccountingHistoryOrderStatus status)
        {
            RaiinfList = raiinfList;
            Status = status;
        }

        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }

        public GetAccountingHistoryOrderStatus Status { get; private set; }
    }
}

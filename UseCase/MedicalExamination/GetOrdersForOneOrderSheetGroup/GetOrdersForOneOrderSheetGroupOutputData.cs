using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetHistory;

namespace UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup
{
    public class GetOrdersForOneOrderSheetGroupOutputData : IOutputData
    {
        public GetOrdersForOneOrderSheetGroupOutputData(int total, List<HistoryKarteOdrRaiinItem> raiinfList, GetOrdersForOneOrderSheetGroupStatus status)
        {
            Total = total;
            RaiinfList = raiinfList;
            Status = status;
        }

        public int Total { get; private set; }
        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
        public GetOrdersForOneOrderSheetGroupStatus Status { get; private set; }

    }
}

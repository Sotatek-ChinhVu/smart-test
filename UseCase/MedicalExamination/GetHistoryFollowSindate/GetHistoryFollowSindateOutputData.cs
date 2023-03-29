using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetHistory;

namespace UseCase.MedicalExamination.GetHistoryFollowSindate
{
    public class GetHistoryFollowSindateOutputData : IOutputData
    {
        public GetHistoryFollowSindateOutputData(List<HistoryKarteOdrRaiinItem> raiinfList, GetHistoryFollowSindateStatus status)
        {
            RaiinfList = raiinfList;
            Status = status;
        }

        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }

        public GetHistoryFollowSindateStatus Status { get; private set; }
    }
}

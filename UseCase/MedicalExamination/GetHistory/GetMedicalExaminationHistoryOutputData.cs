using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryOutputData: IOutputData
    {
        public GetMedicalExaminationHistoryOutputData(int total, List<HistoryKarteOdrRaiinItem> raiinfList, GetMedicalExaminationHistoryStatus status, int startPage)
        {
            Total = total;
            RaiinfList = raiinfList;
            Status = status;
            StartPage = startPage;
        }

        public int Total { get; private set; }
        public int StartPage { get; private set; }
        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
        public GetMedicalExaminationHistoryStatus Status { get; private set; }

    }
}

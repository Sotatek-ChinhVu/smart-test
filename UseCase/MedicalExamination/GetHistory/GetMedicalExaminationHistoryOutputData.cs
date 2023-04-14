using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetDataPrintKarte2;

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

        public GetMedicalExaminationHistoryOutputData(int total, List<HistoryKarteOdrRaiinItem> raiinfList, GetMedicalExaminationHistoryStatus status, int startPage, GetDataPrintKarte2InputData karte2Input)
        {
            Total = total;
            StartPage = startPage;
            RaiinfList = raiinfList;
            Status = status;
            Karte2Input = karte2Input;
        }

        public int Total { get; private set; }
        public int StartPage { get; private set; }
        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
        public GetMedicalExaminationHistoryStatus Status { get; private set; }
        public GetDataPrintKarte2InputData Karte2Input { get; private set; }

    }
}

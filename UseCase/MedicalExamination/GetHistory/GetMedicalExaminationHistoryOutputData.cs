using System.Text.Json.Serialization;
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
            Karte2Input = new();
        }

        public GetMedicalExaminationHistoryOutputData(int total, List<HistoryKarteOdrRaiinItem> raiinfList, GetMedicalExaminationHistoryStatus status, int startPage, GetDataPrintKarte2InputData karte2Input)
        {
            Total = total;
            StartPage = startPage;
            RaiinfList = raiinfList;
            Status = status;
            Karte2Input = karte2Input;
        }

        [JsonPropertyName("total")]
        public int Total { get; private set; }

        [JsonPropertyName("startPage")]
        public int StartPage { get; private set; }

        [JsonPropertyName("raiinfList")]
        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }

        [JsonPropertyName("status")]
        public GetMedicalExaminationHistoryStatus Status { get; private set; }

        [JsonPropertyName("karte2Input")]
        public GetDataPrintKarte2InputData Karte2Input { get; private set; }

    }
}

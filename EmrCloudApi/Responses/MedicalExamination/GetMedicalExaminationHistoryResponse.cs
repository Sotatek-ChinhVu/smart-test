
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetMedicalExaminationHistoryResponse
    {
        public List<HistoryKarteOdrRaiinItem>? KarteOrdRaiins { get; private set; }
        public int Total { get; private set; }
        public int StartPage { get; private set; }
        public GetMedicalExaminationHistoryResponse(List<HistoryKarteOdrRaiinItem>? karteOrdRaiins, int total, int startPage)
        {
            KarteOrdRaiins = karteOrdRaiins;
            Total = total;
            StartPage = startPage;
        }
    }
}

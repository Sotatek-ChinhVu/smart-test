
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class GetMedicalExaminationHistoryResponse
    {
        public List<HistoryKarteOdrRaiinItem>? KarteOrdRaiins { get; set; }
        public GetMedicalExaminationHistoryResponse(List<HistoryKarteOdrRaiinItem>? karteOrdRaiins)
        {
            KarteOrdRaiins = karteOrdRaiins;
        }
    }
}

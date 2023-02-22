using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHistoryOrderResponse
    {
        public GetAccountingHistoryOrderResponse(List<HistoryKarteOdrRaiinItem> raiinfList)
        {
            RaiinfList = raiinfList;
        }

        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
    }
}
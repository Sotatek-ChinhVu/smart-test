using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetOrdersForOneOrderSheetGroupResponse
    {
        public GetOrdersForOneOrderSheetGroupResponse(int total, List<HistoryKarteOdrRaiinItem> raiinfList)
        {
            Total = total;
            RaiinfList = raiinfList;
        }

        public int Total { get; private set; }
        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
    }
}

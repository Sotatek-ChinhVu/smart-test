using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetHistoryFollowSindateResponse
    {
        public GetHistoryFollowSindateResponse(List<HistoryKarteOdrRaiinItem> raiinfList)
        {
            RaiinfList = raiinfList;
        }

        public List<HistoryKarteOdrRaiinItem> RaiinfList { get; private set; }
    }
}
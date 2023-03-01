using UseCase.MedicalExamination.SearchHistory;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class SearchHistoryResponse
    {
        public SearchHistoryResponse(ReceptionItem receptionItem, int searchIndex)
        {
            ReceptionItem = receptionItem;
            SearchIndex = searchIndex;
        }

        public ReceptionItem ReceptionItem { get; private set; }

        public int SearchIndex { get; private set; }
    }
}


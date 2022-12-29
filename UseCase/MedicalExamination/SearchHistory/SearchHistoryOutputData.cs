using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SearchHistory
{
    public class SearchHistoryOutputData : IOutputData
    {
        public SearchHistoryOutputData(ReceptionItem receptionItem, int searchIndex, SearchHistoryStatus status)
        {
            ReceptionItem = receptionItem;
            SearchIndex = searchIndex;
            Status = status;
        }

        public ReceptionItem ReceptionItem { get; private set; }
        public int SearchIndex { get; private set; }
        public SearchHistoryStatus Status { get; private set; }
    }
}

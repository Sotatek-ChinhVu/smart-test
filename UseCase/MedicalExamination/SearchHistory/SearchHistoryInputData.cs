using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SearchHistory
{
    public class SearchHistoryInputData : IInputData<SearchHistoryOutputData>
    {
        public SearchHistoryInputData(int hpId, int userId, long ptId, int sinDate, int currentIndex, int filterId, int isDeleted, string keyWord, int searchType, bool isNext)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            SinDate = sinDate;
            CurrentIndex = currentIndex;
            FilterId = filterId;
            IsDeleted = isDeleted;
            KeyWord = keyWord;
            SearchType = searchType;
            IsNext = isNext;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int CurrentIndex { get; private set; }

        public int FilterId { get; private set; }

        public int IsDeleted { get; private set; }

        public string KeyWord { get; private set; }

        public int SearchType { get; private set; }

        public bool IsNext { get; private set; }
    }
}

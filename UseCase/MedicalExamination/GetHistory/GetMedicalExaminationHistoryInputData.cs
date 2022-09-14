using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryInputData : IInputData<GetMedicalExaminationHistoryOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int UserId { get; private set; }
        public int SearchType { get; private set; }
        public int SearchCategory { get; private set; }
        public string SearchText { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int startPage, int endPage, int userId, int searchType, int searchCategory, string searchText)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            StartPage = startPage;
            EndPage = endPage;
            UserId = userId;
            SearchType = searchType;
            SearchCategory = searchCategory;
            SearchText = searchText;
        }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryInputData : IInputData<GetMedicalExaminationHistoryOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int StartPage { get; private set; }
        public int PageSize { get; private set; }
        public int DeleteConditon { get; private set; }
        public int KarteDeleteHistory { get; private set; }
        public long FilterId { get; private set; }
        public int IsShowApproval { get; private set; }
        public int SearchType { get; private set; }
        public int SearchCategory { get; private set; }
        public string SearchText { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int startPage, int pageSize, int deleteConditon, int karteDeleteHistory, long filterId, int userId, int isShowApproval, int searchType, int searchCategory, string searchText)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            StartPage = startPage;
            PageSize = pageSize;
            DeleteConditon = deleteConditon;
            KarteDeleteHistory = karteDeleteHistory;
            UserId = userId;
            FilterId = filterId;
            IsShowApproval = isShowApproval;
            SearchType = searchType;
            SearchCategory = searchCategory;
            SearchText = searchText;
        }
    }
}

using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryInputData : IInputData<GetMedicalExaminationHistoryOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int DeleteConditon { get; private set; }
        public int KarteDeleteHistory { get; private set; }
        public long FilterId { get; private set; }
        public int IsShowApproval { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int pageIndex, int pageSize, int deleteConditon, int karteDeleteHistory, long filterId, int userId, int isShowApproval)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
            DeleteConditon = deleteConditon;
            KarteDeleteHistory = karteDeleteHistory;
            UserId = userId;
            FilterId = filterId;
            IsShowApproval = isShowApproval;
        }
    }
}

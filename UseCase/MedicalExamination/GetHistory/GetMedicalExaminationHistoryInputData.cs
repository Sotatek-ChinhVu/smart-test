using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryInputData : IInputData<GetMedicalExaminationHistoryOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int Offset { get; private set; }
        public int Limit { get; private set; }
        public int DeleteConditon { get; private set; }
        public long FilterId { get; private set; }
        public int IsShowApproval { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int offset, int limit, int deleteConditon, long filterId, int userId, int isShowApproval)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            Offset = offset;
            Limit = limit;
            DeleteConditon = deleteConditon;
            UserId = userId;
            FilterId = filterId;
            IsShowApproval = isShowApproval;
        }
    }
}

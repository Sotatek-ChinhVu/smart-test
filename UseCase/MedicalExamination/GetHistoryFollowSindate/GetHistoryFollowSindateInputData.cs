using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistoryFollowSindate
{
    public class GetHistoryFollowSindateInputData : IInputData<GetHistoryFollowSindateOutputData>
    {
        public GetHistoryFollowSindateInputData(long ptId, int hpId, int userId, int sinDate, int deleteConditon, long raiinNo, bool isKarteInf)
        {
            PtId = ptId;
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            DeleteConditon = deleteConditon;
            RaiinNo = raiinNo;
            IsKarteInf = isKarteInf;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int DeleteConditon { get; private set; }
        public long RaiinNo { get; private set; }
        public bool IsKarteInf { get; private set; }
    }
}

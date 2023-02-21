using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderInputData : IInputData<GetAccountingHistoryOrderOutputData>
    {
        public GetAccountingHistoryOrderInputData(long ptId, int hpId, int userId, int sinDate, int deleteConditon, long raiinNo)
        {
            PtId = ptId;
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            DeleteConditon = deleteConditon;
            RaiinNo = raiinNo;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int DeleteConditon { get; private set; }
        public long RaiinNo { get; private set; }
    }
}

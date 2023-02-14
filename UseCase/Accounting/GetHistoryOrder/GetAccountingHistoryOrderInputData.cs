using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetHistoryOrder
{
    public class GetAccountingHistoryOrderInputData : IInputData<GetAccountingHistoryOrderOutputData>
    {
        public GetAccountingHistoryOrderInputData(long ptId, int hpId, int userId, int sinDate, int offset, int limit, int deleteConditon, long filterId, int isShowApproval, long raiinNo)
        {
            PtId = ptId;
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
            Offset = offset;
            Limit = limit;
            DeleteConditon = deleteConditon;
            FilterId = filterId;
            IsShowApproval = isShowApproval;
            RaiinNo = raiinNo;
        }

        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public int Offset { get; private set; }
        public int Limit { get; private set; }
        public int DeleteConditon { get; private set; }
        public long FilterId { get; private set; }
        public int IsShowApproval { get; private set; }
        public long RaiinNo { get; private set; }
    }
}

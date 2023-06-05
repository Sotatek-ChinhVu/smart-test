using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.SaveAccounting
{
    public class SaveAccountingInputData : IInputData<SaveAccountingOutputData>
    {
        public SaveAccountingInputData(int hpId, long ptId, int userId, int sinDate, long raiinNo, int sumAdjust, int thisWari, int credit, int payType, string comment, bool isDisCharged, string kaikeiTime)
        {
            HpId = hpId;
            PtId = ptId;
            UserId = userId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SumAdjust = sumAdjust;
            ThisWari = thisWari;
            Credit = credit;
            PayType = payType;
            Comment = comment;
            IsDisCharged = isDisCharged;
            KaikeiTime = kaikeiTime;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int SumAdjust { get; private set; }
        public int ThisWari { get; private set; }
        public int Credit { get; private set; }
        public int PayType { get; private set; }
        public string Comment { get; private set; }
        public bool IsDisCharged { get; private set; }
        public string KaikeiTime { get; private set; }
    }
}

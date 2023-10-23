using Helper.Extension;

namespace UseCase.Lock.Unlock
{
    public class LockPtInfInputItem
    {
        public LockPtInfInputItem(long ptId, string functionName, long ptNum, long sinDate, DateTime lockDate, string machine, string functionCd, long raiinNo, long oyaRaiinNo, int userId)
        {
            PtId = ptId;
            FunctionName = functionName;
            PtNum = ptNum;
            FunctionCd = functionCd;
            SinDate = sinDate;
            LockDate = lockDate;
            Machine = machine;
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
            UserId = userId;
        }

        public long PtId { get; private set; }

        public string FunctionName { get; private set; }

        public long PtNum { get; private set; }

        public long SinDate { get; private set; }

        public DateTime LockDate { get; private set; }

        public string Machine { get; private set; }

        public string FunctionCd { get; private set; }

        public long RaiinNo { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int SinDateInt
        {
            get
            {
                return SinDate.AsInteger();
            }
        }

        public int UserId { get; private set; }
    }
}

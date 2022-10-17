namespace Domain.Models.LockInf
{
    public class ReceptionLockModel
    {
        public ReceptionLockModel(int hpId, long ptId, string functionCd, long sinDate, long raiinNo, long oyaRaiinNo, string machine, int userId, DateTime lockDate)
        {
            HpId = hpId;
            PtId = ptId;
            FunctionCd = functionCd;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
            Machine = machine;
            UserId = userId;
            LockDate = lockDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public string FunctionCd { get; private set; }
        public long SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public long OyaRaiinNo { get; private set; }
        public string Machine { get; private set; }
        public int UserId { get; private set; }
        public DateTime LockDate { get; private set; }
    }
}

namespace UseCase.Lock.Unlock
{
    public class PtInfInputItem
    {
        public PtInfInputItem(long ptId, string functionCd, long sinDate, long raiinNo, long oyaRaiinNo)
        {
            PtId = ptId;
            FunctionCd = functionCd;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
        }

        public long PtId {  get; private set; }

        public string FunctionCd { get; private set; }

        public long SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long OyaRaiinNo { get; private set; }
    }
}

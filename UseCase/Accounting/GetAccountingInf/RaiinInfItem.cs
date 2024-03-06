namespace UseCase.Accounting.GetAccountingInf
{
    public class RaiinInfItem
    {
        public RaiinInfItem(int hokenId, long raiinNo)
        {
            HokenId = hokenId;
            RaiinNo = raiinNo;
        }

        public int HokenId { get; private set; }
        public long RaiinNo { get; private set; }
    }
}

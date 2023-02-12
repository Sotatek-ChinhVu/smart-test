namespace Domain.Models.Accounting
{
    public class WarningMemoModel
    {


        public WarningMemoModel()
        {
            Memo = string.Empty;
        }

        public WarningMemoModel(long raiinNo, string memo, int color)
        {
            RaiinNo = raiinNo;
            Memo = memo;
            Color = color;
        }

        public long RaiinNo { get; private set; }
        public string Memo { get; private set; }
        public int Color { get; private set; }
    }
}

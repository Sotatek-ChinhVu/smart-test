namespace Domain.Models.Accounting
{
    public class CalcLogModel
    {
        public CalcLogModel(long raiinNo, int logSbt, string text)
        {
            RaiinNo = raiinNo;
            LogSbt = logSbt;
            Text = text;
        }

        public long RaiinNo { get; private set; }

        public int LogSbt { get; private set; }

        public string Text { get; private set; }
    }
}

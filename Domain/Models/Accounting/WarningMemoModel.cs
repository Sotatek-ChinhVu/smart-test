namespace Domain.Models.Accounting
{
    public class WarningMemoModel
    {
        public WarningMemoModel(string memo, int color)
        {
            Memo = memo;
            Color = color;
        }

        public WarningMemoModel()
        {
            Memo = string.Empty;
        }

        public string Memo { get; private set; }
        public int Color { get; private set; }
    }
}

namespace Domain.Models.Accounting
{
    public class WarningMemoDto
    {
        public WarningMemoDto()
        {
            Memo = string.Empty;
        }

        public WarningMemoDto(long raiinNo, string memo, int color)
        {
            RaiinNo = raiinNo;
            Memo = memo;
            Color = color;
        }

        public WarningMemoDto(string memo, int color)
        {
            Memo = memo;
            Color = color;
        }

        public long RaiinNo { get; private set; }
        public string Memo { get; private set; }
        public int Color { get; private set; }
    }
}

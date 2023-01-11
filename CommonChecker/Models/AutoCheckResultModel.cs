namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class AutoCheckResultModel
    {
        public int RpNo { get; set; }

        public int RowNo { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;
    }
}

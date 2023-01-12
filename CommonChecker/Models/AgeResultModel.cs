namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class AgeResultModel
    {
        public int RpNo { get; set; }

        public int RpEdaNo { get; set; }

        public int RowNo { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string YjCd { get; set; } = string.Empty;

        public string TenpuLevel { get; set; } = string.Empty;

        public string AttentionCmtCd { get; set; } = string.Empty;

        public string WorkingMechanism { get; set; } = string.Empty;
    }
}

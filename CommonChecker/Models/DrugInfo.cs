namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DrugInfo
    {
        public string Id { get; set; } = string.Empty;
        public string ItemCD { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Suryo { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public double TermVal { get; set; }
        public int SinKouiKbn { get; set; }
        public double UsageQuantity { get; set; }
    }
}

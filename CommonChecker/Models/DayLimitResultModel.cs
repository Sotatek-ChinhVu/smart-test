using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DayLimitResultModel : OrderInforResultModel
    {
        public string YjCd { get; set; } = string.Empty;
        public double UsingDay { get; set; }
        public double LimitDay { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemCd { get; set; } = string.Empty;
    }
}

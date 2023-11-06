using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DayLimitResultModel : OrderInforResultModel
    {
        public string YjCd = string.Empty;
        public double UsingDay;
        public double LimitDay;
        public string ItemName = string.Empty;
        public string ItemCd = string.Empty;
    }
}

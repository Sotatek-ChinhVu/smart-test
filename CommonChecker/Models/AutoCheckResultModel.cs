using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class AutoCheckResultModel : OrderInforResultModel
    {
        public string ItemCd { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;
    }
}

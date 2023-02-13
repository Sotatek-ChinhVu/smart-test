using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class FoodAllergyResultModel : OrderInforResultModel
    {
        public long PtId { get; set; }

        public string AlrgyKbn { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public string YjCd { get; set; } = string.Empty;

        public string TenpuLevel { get; set; } = string.Empty;

        public string AttentionCmt { get; set; } = string.Empty;

        public string WorkingMechanism { get; set; } = string.Empty;
    }
}

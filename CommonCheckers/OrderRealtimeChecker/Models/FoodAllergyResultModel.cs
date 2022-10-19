namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class FoodAllergyResultModel
    {
        public long PtId { get; set; }

        public string AlrgyKbn { get; set; }

        public string ItemCd { get; set; }

        public string YjCd { get; set; }

        public string TenpuLevel { get; set; }

        public string AttentionCmt { get; set; }

        public string WorkingMechanism { get; set; }
    }
}

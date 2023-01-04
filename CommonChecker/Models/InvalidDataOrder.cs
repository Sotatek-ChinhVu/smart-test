namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class InvalidDataOrder
    {
        public ErrorType ErrorType { get; set; }

        public string ItemName { get; set; } = string.Empty;
    }
    public enum ErrorType
    {
        Expired = 1,
        Quantity = 2,
        QuantityLimit = 3,
        Usage = 4,
        InjectionUsage = 5,
        BukantuItem = 6,
        RefillQuantityLimit = 7,
    }
}

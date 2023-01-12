using CommonCheckers.OrderRealtimeChecker.Enums;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class UnitCheckInfoModel
    {
        public RealtimeCheckerType CheckerType { get; set; }

        public int Sinday { get; set; }

        public long PtId { get; set; }

        public bool IsError { get; set; }

        public object ErrorInfo { get; set; } = string.Empty;
    }
}

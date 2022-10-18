namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DosageResultModel
    {
        public string ItemCd { get; set; }

        public string ItemName { get; set; }

        public string YjCd { get; set; }

        public double CurrentValue { get; set; }

        public double SuggestedValue { get; set; }

        public string UnitName { get; set; }

        public DosageLabelChecking LabelChecking { get; set; }

        public bool IsFromUserDefined { get; set; }
    }

    public enum DosageLabelChecking
    {
        // 一回量／最小値
        OneMin,

        // 一回量／最大値
        OneMax,

        // 一回量／上限値
        OneLimit,

        // 一日量／最小値
        DayMin,

        // 一日量／最大値
        DayMax,

        // 一日量／上限値
        DayLimit,

        // 期間上限
        TermLimit
    }
}

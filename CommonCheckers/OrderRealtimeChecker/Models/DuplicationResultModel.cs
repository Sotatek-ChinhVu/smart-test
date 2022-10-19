namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DuplicationResultModel
    {
        public int Level { get; set; }

        public string ItemCd { get; set; }

        public string DuplicatedItemCd { get; set; }

        public bool IsIppanCdDuplicated { get; set; } = false;

        public bool IsComponentDuplicated { get; set; } = false;

        public string IppanCode { get; set; }

        public string SeibunCd { get; set; }

        public string AllergySeibunCd { get; set; }

        public string Tag { get; set; }
    }
}

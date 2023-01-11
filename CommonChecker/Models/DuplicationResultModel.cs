namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DuplicationResultModel
    {
        public int RpNo { get; set; }

        public int RowNo { get; set; }

        public int Level { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string DuplicatedItemCd { get; set; } = string.Empty;

        public bool IsIppanCdDuplicated { get; set; } = false;

        public bool IsComponentDuplicated { get; set; } = false;

        public string IppanCode { get; set; } = string.Empty;

        public string SeibunCd { get; set; } = string.Empty;

        public string AllergySeibunCd { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;
    }
}

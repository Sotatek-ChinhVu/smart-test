﻿namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DrugAllergyResultModel
    {
        public int RpNo { get; set; }

        public int RpEdaNo { get; set; }

        public int RowNo { get; set; }

        public int Level { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public string YjCd { get; set; } = string.Empty;

        public string AllergyItemCd { get; set; } = string.Empty;

        public string AllergyYjCd { get; set; } = string.Empty;

        public string SeibunCd { get; set; } = string.Empty;

        public string AllergySeibunCd { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public string SeqNo { get; set; } = string.Empty;
    }
}

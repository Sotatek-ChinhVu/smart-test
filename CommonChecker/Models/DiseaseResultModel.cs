﻿using CommonChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DiseaseResultModel : OrderInforResultModel
    {
        public int DiseaseType { get; set; } //0:現疾患 1:既往歴 2:家族歴

        public string ItemCd { get; set; } = string.Empty;

        public string YjCd { get; set; } = string.Empty;

        public int TenpuLevel { get; set; }

        public string ByotaiCd { get; set; } = string.Empty;

        public string CmtCd { get; set; } = string.Empty;

        public string KijyoCd { get; set; } = string.Empty;
    }
}

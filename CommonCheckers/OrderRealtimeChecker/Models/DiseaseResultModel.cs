namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class DiseaseResultModel
    {
        public int DiseaseType { get; set; } //0:現疾患 1:既往歴 2:家族歴

        public string ItemCd { get; set; }

        public string YjCd { get; set; }

        public int TenpuLevel { get; set; }

        public string ByotaiCd { get; set; }

        public string CmtCd { get; set; }

        public string KijyoCd { get; set; }
    }
}

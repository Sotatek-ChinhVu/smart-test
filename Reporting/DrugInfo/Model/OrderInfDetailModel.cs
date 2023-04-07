namespace Reporting.DrugInfo.Model
{
    public class OrderInfDetailModel
    {
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public int RowNo { get; set; }
        public int SinKouiKbn { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Suryo { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public int UnitSBT { get; set; }
        public double TermVal { get; set; }
        public int YohoKbn { get; set; }
    }
}

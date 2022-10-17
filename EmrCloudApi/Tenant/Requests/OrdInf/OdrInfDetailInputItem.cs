namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class OdrInfDetailInputItem
    {
        public int RowNo { get; set; }
        public int SinKouiKbn { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Suryo { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public int KohatuKbn { get; set; }
        public int SyohoKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public string Bunkatu { get; set; } = string.Empty;
        public string CmtName { get; set; } = string.Empty;
        public string CmtOpt { get; set; } = string.Empty;
    }
}

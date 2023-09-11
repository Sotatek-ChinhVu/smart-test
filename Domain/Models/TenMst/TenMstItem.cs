namespace Domain.Models.TenMst
{
    public class TenMstItemModel
    {
        public string ItemCd { get; set; }
        public int SinKouiKbn { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OdrUnitName { get; set; } = string.Empty;
        public string CnvUnitName { get; set; } = string.Empty;
        public int IsNodspRece { get; set; }
        public int YohoKbn { get; set; }
        public double OdrTermVal { get; set; }
        public double CnvTermVal { get; set; }
        public string YjCd { get; set; } = string.Empty;
        public string KensaItemCd { get; set; } = string.Empty;
        public int KensaItemSeqNo { get; set; }
        public int KohatuKbn { get; set; }
        public double Ten { get; set; }
        public int HandanGrpKbn { get; set; }
        public string IpnNameCd { get; set; } = string.Empty;
        public int CmtCol1 { get; set; }
        public int CmtCol2 { get; set; }
        public int CmtCol3 { get; set; }
        public int CmtCol4 { get; set; }
        public int CmtColKeta1 { get; set; }
        public int CmtColKeta2 { get; set; }
        public int CmtColKeta3 { get; set; }
        public int CmtColKeta4 { get; set; }
        public string MinAge { get; set; } = string.Empty;
        public string MaxAge { get; set; } = string.Empty;
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public string MasterSbt { get; set; } = string.Empty;
        public int BuiKbn { get; set; }
        public string CdKbn { get; set; } = string.Empty;
        public int CdKbnno { get; set; }
        public int CdEdano { get; set; }
        public string Kokuji1 { get; set; } = string.Empty;
        public string Kokuji2 { get; set; } = string.Empty;
        public int DrugKbn { get; set; }
        public string ReceName { get; set; } = string.Empty;
        public string SanteiItemCd { get; set; } = string.Empty;
        public int JihiSbt { get; set; }
        public int IsDeleted { get; set; }
    }
}

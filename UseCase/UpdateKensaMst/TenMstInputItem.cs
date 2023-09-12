namespace UseCase.UpdateKensaMst
{
    public class TenMstInputItem
    {
        public TenMstInputItem(string itemCd, int sinKouiKbn, string name, string odrUnitName, string cnvUnitName, int isNodspRece, int yohoKbn, double odrTermVal, double cnvTermVal, string yjCd, string kensaItemCd, int kensaItemSeqNo
                          , int kohatuKbn, double ten, int handanGrpKbn, string ipnNameCd, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, string minAge
                          , string maxAge, int startDate, int endDate, string masterSbt, int buiKbn, string cdKbn, int cdKbnNo, int cdEdano, string kokuji1, string kokuji2, int drugKbn, string receName, string santeiItemCd, int jihiSbt, int isDeleted)
        {
            ItemCd = itemCd;
            SinKouiKbn = sinKouiKbn;
            Name = name;
            OdrUnitName = odrUnitName;
            CnvUnitName = cnvUnitName;
            IsNodspRece = isNodspRece;
            YohoKbn = yohoKbn;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            KohatuKbn = kohatuKbn;
            Ten = ten;
            HandanGrpKbn = handanGrpKbn;
            IpnNameCd = ipnNameCd;
            CmtCol1 = cmtCol1;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            MinAge = minAge;
            MaxAge = maxAge;
            StartDate = startDate;
            EndDate = endDate;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            CdKbn = cdKbn;
            CdKbnno = cdKbnNo;
            CdEdano = cdEdano;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            DrugKbn = drugKbn;
            ReceName = receName;
            SanteiItemCd = santeiItemCd;
            JihiSbt = jihiSbt;
            IsDeleted = isDeleted;
        }

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

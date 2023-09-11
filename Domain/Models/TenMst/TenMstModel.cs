using static Helper.Constants.TenMstConst;

namespace Domain.Models.TenMst
{
    public class TenMstModel
    {
        public TenMstModel(string itemCd, int sinKouiKbn, string name, string odrUnitName, string cnvUnitName, int isNodspRece, int yohoKbn, double odrTermVal, double cnvTermVal, string yjCd, string kensaItemCd, int kensaItemSeqNo
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

        public string ItemCd { get; private set; }
        public int SinKouiKbn { get; private set; }
        public string Name { get; private set; }
        public string OdrUnitName { get; private set; }
        public string CnvUnitName { get; private set; }
        public int IsNodspRece { get; private set; }
        public int YohoKbn { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; }
        public string KensaItemCd { get; private set; } 
        public int KensaItemSeqNo { get; private set; }
        public int KohatuKbn { get; private set; }
        public double Ten { get; private set; }
        public int HandanGrpKbn { get; private set; }
        public string IpnNameCd { get; private set; } 
        public int CmtCol1 { get; private set; }
        public int CmtCol2 { get; private set; }
        public int CmtCol3 { get; private set; }
        public int CmtCol4 { get; private set; }
        public int CmtColKeta1 { get; private set; }
        public int CmtColKeta2 { get; private set; }
        public int CmtColKeta3 { get; private set; }
        public int CmtColKeta4 { get; private set; }
        public string MinAge { get; private set; }
        public string MaxAge { get; private set; } 
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string MasterSbt { get; private set; }
        public int BuiKbn { get; private set; }
        public string CdKbn { get; private set; }
        public int CdKbnno { get; private set; }
        public int CdEdano { get; private set; }
        public string Kokuji1 { get; private set; } 
        public string Kokuji2 { get; private set; } 
        public int DrugKbn { get; private set; }
        public string ReceName { get; private set; }
        public string SanteiItemCd { get; private set; }
        public int JihiSbt { get; private set; }
        public int IsDeleted { get; private set; }

        public ValidationStatus Validation()
        {
            if (MasterSbt.Length > 1)
            {
                return ValidationStatus.InvalidMasterSbt;
            }

            if (ItemCd.Length > 10)
            {
                return ValidationStatus.InvalidItemCd;
            }

            if (MinAge.Length > 2)
            {
                return ValidationStatus.InvalidMinAge;
            }

            if (MaxAge.Length > 2)
            {
                return ValidationStatus.InvalidMaxAge;
            }

            if (CdKbn.Length > 1)
            {
                return ValidationStatus.InvalidCdKbn;
            }

            if (Kokuji1.Length > 1)
            {
                return ValidationStatus.InvalidKokuji1;
            }

            if (Kokuji2.Length > 1)
            {
                return ValidationStatus.InvalidKokuji2;
            }

            return ValidationStatus.Valid;
        }
    }
}

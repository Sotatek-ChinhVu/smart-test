using System.Collections.ObjectModel;

namespace Domain.Models.MstItem
{
    public class TenMstModel
    {
        public ObservableCollection<TenMstModel> ListGenDate { get; set; }
        public TenMstModel(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1
                          , string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6
                          , string kanaName7, int startDate, int endDate, double defaultVal, string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch)
        {
            SinKouiKbn = sinKouiKbn;
            MasterSbt = masterSbt;
            ItemCd = itemCd;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            Ten = ten;
            Name = name;
            ReceName = receName;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
            StartDate = startDate;
            EndDate = endDate;
            DefaultVal = defaultVal;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
        }
        public int HpId { get; private set; }
        public double DefaultVal { get; private set; }
        public int SanteigaiKbn { get; private set; }
        public int IsNoSearch { get; private set; }
        public string KanaName1 { get; private set; }
        public string KanaName2 { get; private set; }
        public string KanaName3 { get; private set; }
        public string KanaName4 { get; private set; }
        public string KanaName5 { get; private set; }
        public string KanaName6 { get; private set; }
        public string KanaName7 { get; private set; }
        public string OdrItemName { get; private set; } = string.Empty;
        public string OldSanteiItemCd { get; set; }
        public string ItemCd { get; private set; }
        public int SinKouiKbn { get; private set; }
        public string Name { get; private set; }
        public string OdrUnitName { get; private set; }
        public string CnvUnitName { get; private set; } = string.Empty;
        public int IsNodspRece { get; private set; }
        public int YohoKbn { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; } = string.Empty;
        public string KensaItemCd { get; private set; }
        public int KensaItemSeqNo { get; private set; }
        public int KohatuKbn { get; private set; }
        public double Ten { get; private set; }
        public int HandanGrpKbn { get; private set; }
        public string IpnNameCd { get; private set; } = string.Empty;
        public int CmtCol1 { get; private set; }
        public int CmtCol2 { get; private set; }
        public int CmtCol3 { get; private set; }
        public int CmtCol4 { get; private set; }
        public int CmtColKeta1 { get; private set; }
        public int CmtColKeta2 { get; private set; }
        public int CmtColKeta3 { get; private set; }
        public int CmtColKeta4 { get; private set; }
        public string MinAge { get; private set; } = string.Empty;
        public string MaxAge { get; private set; } = string.Empty;
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string MasterSbt { get; private set; } = string.Empty;
        public int BuiKbn { get; private set; }
        public string CdKbn { get; private set; } = string.Empty;
        public int CdKbnNo { get; private set; }
        public int CdEdano { get; private set; }
        public string Kokuji1 { get; private set; } = string.Empty;
        public string Kokuji2 { get; private set; } = string.Empty;
        public string DrugKbn { get; private set; } = string.Empty;
        public string ReceName { get; private set; }
        public string SanteiItemCd { get; private set; }
        public int JihiSbt { get; private set; }
    }
}

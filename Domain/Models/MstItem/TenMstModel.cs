using Helper.Common;
using Helper.Extension;
using System.Collections.ObjectModel;

namespace Domain.Models.MstItem
{
    public class TenMstModel
    {
        public ObservableCollection<TenMstModel> ListGenDate { get; set; }
        public TenMstModel(IEnumerable<int> sinKouiKbn, IEnumerable<string> masterSbt, IEnumerable<string> itemCd, IEnumerable<string> kensaItemCd, IEnumerable<int> kensaItemSeqNo, IEnumerable<double> ten, IEnumerable<string> name
                          , IEnumerable<string> receName, IEnumerable<string> kanaName1, IEnumerable<string> kanaName2, IEnumerable<string> kanaName3, IEnumerable<string> kanaName4, IEnumerable<string> kanaName5, IEnumerable<string> kanaName6
                          , IEnumerable<string> kanaName7, IEnumerable<int> startDate, IEnumerable<int> endDate, IEnumerable<double> defaultVal, IEnumerable<string> odrUnitName
                          , IEnumerable<string> santeiItemCd, IEnumerable<int> santeigaiKbn, IEnumerable<int> isNoSearch)
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
        public IEnumerable<double> DefaultVal { get; private set; }
        public IEnumerable<int> SanteigaiKbn { get; private set; }
        public IEnumerable<int> IsNoSearch { get; private set; }
        public IEnumerable<string> KanaName1 { get; private set; }
        public IEnumerable<string> KanaName2 { get; private set; }
        public IEnumerable<string> KanaName3 { get; private set; } 
        public IEnumerable<string> KanaName4 { get; private set; } 
        public IEnumerable<string> KanaName5 { get; private set; } 
        public IEnumerable<string> KanaName6 { get; private set; }
        public IEnumerable<string> KanaName7 { get; private set; } 
        public string OdrItemName { get; private set; } = string.Empty;
        public IEnumerable<string> OldSanteiItemCd { get; set; }
        public IEnumerable<string> ItemCd { get; private set; }
        public IEnumerable<int> SinKouiKbn { get; private set; }
        public IEnumerable<string> Name { get; private set; }
        public IEnumerable<string> OdrUnitName { get; private set; } 
        public string CnvUnitName { get; private set; } = string.Empty;
        public int IsNodspRece { get; private set; }
        public int YohoKbn { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; } = string.Empty;
        public IEnumerable<string> KensaItemCd { get; private set; }
        public IEnumerable<int> KensaItemSeqNo { get; private set; }
        public int KohatuKbn { get; private set; }
        public IEnumerable<double> Ten { get; private set; }
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
        public IEnumerable<int> StartDate { get; private set; }
        public IEnumerable<int> EndDate { get; private set; }
        public IEnumerable<string> MasterSbt { get; private set; }
        public int BuiKbn { get; private set; }
        public string CdKbn { get; private set; } = string.Empty;
        public int CdKbnNo { get; private set; }
        public int CdEdano { get; private set; }
        public string Kokuji1 { get; private set; } = string.Empty;
        public string Kokuji2 { get; private set; } = string.Empty;
        public string DrugKbn { get; private set; } = string.Empty;
        public IEnumerable<string> ReceName { get; private set; }
        public IEnumerable<string> SanteiItemCd { get; private set; }
        public int JihiSbt { get; private set; }
    }
}

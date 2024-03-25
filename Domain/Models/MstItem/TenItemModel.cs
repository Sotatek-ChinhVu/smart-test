using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.Text.Json.Serialization;
using static Helper.Constants.TenMstConst;

namespace Domain.Models.MstItem
{
    public class TenItemModel
    {

        [JsonConstructor]
        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, string ipnName, int isDeleted, int handanGrpKbn, bool isKensaMstEmpty)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IpnName = ipnName;
            IsDeleted = isDeleted;
            HandanGrpKbn = handanGrpKbn;
            IsKensaMstEmpty = isKensaMstEmpty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            ReceName = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, string ipnName, int isDeleted, int handanGrpKbn, bool isKensaMstEmpty, double yakka, bool isGetPriceInYakka)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IpnName = ipnName;
            IsDeleted = isDeleted;
            HandanGrpKbn = handanGrpKbn;
            IsKensaMstEmpty = isKensaMstEmpty;
            Yakka = yakka;
            IsGetPriceInYakka = isGetPriceInYakka;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            ReceName = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, string ipnName, int isDeleted)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IpnName = ipnName;
            IsDeleted = isDeleted;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            ReceName = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, int modeStatus)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            ModeStatus = modeStatus;
            IpnName = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            ReceName = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel()
        {
            HpId = 0;
            ItemCd = string.Empty;
            RousaiKbn = 0;
            KanaName1 = string.Empty;
            Name = string.Empty;
            KohatuKbn = 0;
            MadokuKbn = 0;
            KouseisinKbn = 0;
            OdrUnitName = string.Empty;
            EndDate = 0;
            DrugKbn = 0;
            MasterSbt = string.Empty;
            BuiKbn = 0;
            Ten = 0;
            TenId = 0;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IsAdopted = 0;
            CmtCol1 = 0;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            YohoKbn = 0;
            IpnCD = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            SanteiItemCd = string.Empty;
            CnvTermVal = 0;
            OdrTermVal = 0;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnName = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            ReceName = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, string minAge, string maxAge, string santeiItemCd, int startDate, int endDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            IpnCD = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnNameCd = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            MasterSbt = string.Empty;
            OdrUnitName = string.Empty;
            Name = string.Empty;
            KanaName1 = string.Empty;
            StartDate = startDate;
            EndDate = endDate;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnName = string.Empty;
            ReceName = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1
                          , string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, int startDate, int endDate, double defaultValue
                          , string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch, List<string> listGenDate)
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
            DefaultValue = defaultValue;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnCD = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = listGenDate;
            SetItemType();
        }

        public TenItemModel(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1
                          , string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, int startDate, int endDate, double defaultValue
                          , string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch)
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
            DefaultValue = defaultValue;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnCD = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1
                          , string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, int startDate, int endDate, double defaultValue
                          , string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch, int isDeleted)
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
            DefaultValue = defaultValue;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnCD = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
            IsDeleted = isDeleted;
        }

        public TenItemModel(string itemCd, int sinKouiKbn, string name, string odrUnitName, string cnvUnitName, int isNodspRece, int yohoKbn, double odrTermVal, double cnvTermVal, string yjCd, string kensaItemCd, int kensaItemSeqNo
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
            KanaName1 = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnCD = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string name, string receName, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, string ipnName, int isDeleted, int handanGrpKbn, bool isKensaMstEmpty, double yakka, bool isGetPriceInYakka, int kasan1, int kasan2)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IpnName = ipnName;
            IsDeleted = isDeleted;
            HandanGrpKbn = handanGrpKbn;
            IsKensaMstEmpty = isKensaMstEmpty;
            Yakka = yakka;
            IsGetPriceInYakka = isGetPriceInYakka;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
            KensaItemCd = string.Empty;
            ReceName = receName;
            CdKbn = string.Empty;
            ListGenDate = new();
        }

        public TenItemModel(int hpId, string itemCd, string kokuji1, string kokuji2, int sinKouiKbn, string name, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string odrUnitName, string cnvUnitName, int isNodspRece, int yohoKbn, double odrTermVal, double cnvTermVal, string yjCd, string kensaItemCd, int kensaItemSeqNo, int kohatuKbn, double ten, int handanGrpKbn, string ipnNameCd, int isAdopted, int drugKbn, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, string masterSbt, double defaultValue)
        {
            HpId = hpId;
            ItemCd = itemCd;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            SinKouiKbn = sinKouiKbn;
            Name = name;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
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
            IsAdopted = isAdopted;
            DrugKbn = drugKbn;
            CmtCol1 = cmtCol1;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            MasterSbt = masterSbt;
            DefaultValue = defaultValue;
            ReceName = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnCD = string.Empty;
            MaxAge = string.Empty;
            MinAge = string.Empty;
            SanteiItemCd = string.Empty;
            ListGenDate = new();
            CdKbn = string.Empty;
        }

        public TenItemModel(string itemCd, double ten, int handanGrpKbn, int endDate, string kensaItemCd, int kensaItemSeqNo, string ipnNameCd)
        {
            Ten = ten;
            HandanGrpKbn = handanGrpKbn;
            EndDate = endDate;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            IpnNameCd = ipnNameCd;
            ItemCd = itemCd;
            KanaName1 = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            ReceName = string.Empty;
            Name = string.Empty;
            OdrUnitName = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnCD = string.Empty;
            MaxAge = string.Empty;
            MinAge = string.Empty;
            SanteiItemCd = string.Empty;
            ListGenDate = new();
            CdKbn = string.Empty;
        }

        public TenItemModel(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1
                     , string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, int startDate, int endDate, double defaultValue
                     , string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch, List<string> listGenDate, DateTime createDate)
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
            DefaultValue = defaultValue;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnCD = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = listGenDate;
            SetItemType();
            CreateDate = createDate;
        }

        public TenItemModel(string itemCd)
        {
            ItemCd = itemCd;
        }
        public TenItemModel(string itemCd, string odrUnitName = "", string cnvUnitName ="", int sinKouiKbn =0)
        {
            ItemCd = itemCd;
            OdrUnitName = odrUnitName;
            CnvUnitName = cnvUnitName;
            SinKouiKbn =sinKouiKbn;
        }

        public TenItemModel(int hpId, string itemCd, string minAge, string maxAge, string santeiItemCd, int startDate, int endDate, int buiKbn)
        {
            HpId = hpId;
            ItemCd = itemCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            IpnCD = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnNameCd = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            MasterSbt = string.Empty;
            OdrUnitName = string.Empty;
            Name = string.Empty;
            KanaName1 = string.Empty;
            StartDate = startDate;
            EndDate = endDate;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnName = string.Empty;
            ReceName = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            KensaItemCd = string.Empty;
            CdKbn = string.Empty;
            ListGenDate = new();
            BuiKbn = buiKbn;
        }
        public int HpId { get; private set; }

        public string ReceName { get; private set; }

        public int IsNodspRece { get; private set; }

        public int IsNoSearch { get; private set; }

        public int SanteigaiKbn { get; private set; }

        public int CdKbnno { get; private set; }

        public int CdEdano { get; private set; }

        public int JihiSbt { get; private set; }

        public string KanaName2 { get; private set; }

        public string KanaName3 { get; private set; }

        public string KanaName4 { get; private set; }

        public string KanaName5 { get; private set; }

        public string KanaName6 { get; private set; }

        public string KanaName7 { get; private set; }

        public string KensaItemCd { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public string ItemCd { get; private set; }

        public int RousaiKbn { get; private set; }

        public string KanaName1 { get; private set; }

        public string Name { get; private set; }

        public int KohatuKbn { get; private set; }

        public int MadokuKbn { get; private set; }

        public int KouseisinKbn { get; private set; }

        public string OdrUnitName { get; private set; }

        public int EndDate { get; private set; }

        public int DrugKbn { get; private set; }

        public string MasterSbt { get; private set; }

        public int BuiKbn { get; private set; }

        public int IsAdopted { get; private set; }

        public double Ten { get; private set; }

        public int TenId { get; private set; }

        public string KensaMstCenterItemCd1 { get; private set; }

        public string KensaMstCenterItemCd2 { get; private set; }

        public int CmtCol1 { get; private set; }

        public string IpnNameCd { get; private set; }

        public int SinKouiKbn { get; private set; }

        public string YjCd { get; private set; }

        public string CnvUnitName { get; private set; }

        public int StartDate { get; private set; }

        public int YohoKbn { get; private set; }

        public int CmtColKeta1 { get; private set; }

        public int CmtColKeta2 { get; private set; }

        public int CmtColKeta3 { get; private set; }

        public int CmtColKeta4 { get; private set; }

        public int CmtCol2 { get; private set; }

        public int CmtCol3 { get; private set; }

        public int CmtCol4 { get; private set; }

        public string IpnCD { get; private set; }

        public string MaxAge { get; private set; }

        public string MinAge { get; private set; }

        public string SanteiItemCd { get; private set; }

        public double OdrTermVal { get; private set; }

        public double CnvTermVal { get; private set; }

        public double DefaultValue { get; private set; }

        public string Kokuji1 { get; private set; }

        public string Kokuji2 { get; private set; }

        public int HandanGrpKbn { get; private set; }

        public int ModeStatus { get; private set; }

        public string IpnName { get; private set; } = string.Empty;

        public int IsDeleted { get; private set; }

        public bool IsKensaMstEmpty { get; private set; }

        public double Yakka { get; private set; }

        public bool IsGetPriceInYakka { get; private set; }

        public List<string> ListGenDate { get; private set; }

        public int Kasan1 { get; private set; }

        public int Kasan2 { get; private set; }

        public DateTime CreateDate { get; private set; }

        public string RousaiKbnDisplay
        {
            get
            {
                switch (RousaiKbn)
                {
                    case 1:
                        return "〇";
                    case 3:
                        return "ア";
                    default:
                        return "";
                }
            }
        }

        public string KouseisinKbnDisplay
        {
            get
            {
                return KouseisinKbn switch
                {
                    1 => "抗不",
                    2 => "睡眠",
                    3 => "うつ",
                    4 => "抗精",
                    _ => "",
                };
            }
        }

        public string KubunToDisplay
        {
            get
            {
                return MadokuKbn switch
                {
                    1 => "麻",
                    2 => "毒",
                    3 => "覚",
                    5 => "向",
                    _ => "",
                };
            }
        }

        public string KohatuKbnDisplay
        {
            get
            {
                if (KohatuKbn == 1)
                {
                    return "後";
                }
                else if (KohatuKbn == 2)
                {
                    return "〇";
                }
                return string.Empty;
            }
        }

        public string KensaCenterItemCDDisplay
        {
            get
            {
                if (string.IsNullOrEmpty(KensaMstCenterItemCd1)) return KensaMstCenterItemCd2;

                if (string.IsNullOrEmpty(KensaMstCenterItemCd2)) return KensaMstCenterItemCd1;

                if (!string.IsNullOrEmpty(KensaMstCenterItemCd1) && !string.IsNullOrEmpty(KensaMstCenterItemCd2))
                {
                    return string.Format("{0}/{1}", KensaMstCenterItemCd1, KensaMstCenterItemCd1);
                }

                return string.Empty;
            }
        }

        public string TenDisplay
        {
            get
            {
                if (Ten == 0) return Ten.ToString();

                if (new[] { 1, 2, 4 }.Contains(TenId))
                {
                    return "￥" + Ten.ToString();
                }
                if (new[] { 5, 6 }.Contains(TenId))
                {
                    return Ten.ToString() + "%";
                }

                return Ten.ToString();
            }
        }

        public string KouiName { get => BuildKouiName(ItemCd, DrugKbn, MasterSbt, BuiKbn, SinKouiKbn); }

        private static string BuildKouiName(string itemCd, int drugKbn, string masterSbt, int buiKbn, int sinKouiKbn)
        {
            string rs = "";
            var sinKouiCollection = new SinkouiCollection();
            var itemKoui = sinKouiCollection.FirstOrDefault(x => x.SinKouiCd == sinKouiKbn);
            if (itemKoui != null)
            {
                rs = itemKoui.SinkouiName;
            }
            if (itemCd == ItemCdConst.GazoDensibaitaiHozon)
            {
                rs = "フィルム";
            }
            if (drugKbn > 0)
            {
                switch (drugKbn)
                {
                    case 1:
                        rs = "内用";
                        break;
                    case 3:
                        rs = "薬剤他";
                        break;
                    case 4:
                        rs = "注射";
                        break;
                    case 6:
                        rs = "外用";
                        break;
                    case 8:
                        rs = "歯科薬";
                        break;
                }

            }
            else if (masterSbt == "T")
            {
                rs = "特材";
            }
            if (buiKbn > 0)
            {
                rs = "部位";
            }
            return rs;
        }

        public string CdKbn { get; private set; }

        public string FormattedStartDate
        {
            get => StartDate > 0 ? CIUtil.SDateToShowSDate(StartDate) : "0";
            set => StartDate = value.Replace("/", string.Empty).AsInteger();
        }

        public string FormattedEndDate
        {
            get => EndDate != 99999999 ? CIUtil.SDateToShowSDate(EndDate) : "9999/99/99";
            set => EndDate = value.Replace("/", string.Empty).AsInteger();
        }

        public bool IsDefault => CheckDefaultValue();

        public string ReadOnlyStartDate
        {
            get
            {
                if (IsDefault) return string.Empty;
                return StartDate > 0 ? CIUtil.SDateToShowSDate(StartDate) : "0";
            }
        }

        public bool IsSanteiItem => SanteigaiKbn != 1 && SanteiItemCd != "9999999999";

        public int ItemType { get; private set; }

        private void SetItemType()
        {
            if (IsDefault)
            {
                ItemType = -1;
            }
            else
            {
                ItemType = ItemCd.AsString().StartsWith("IGE") ? 1 : 0;
            }
        }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }

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

using Domain.Models.MstItem;
using System.Text.Json.Serialization;

namespace EmrCloudApi.Responses.MstItem
{
    public class TenItemDto
    {
        public TenItemDto(TenItemModel model)
        {
            HpId = model.HpId;
            ItemCd = model.ItemCd;
            RousaiKbn = model.RousaiKbn;
            KanaName1 = model.KanaName1;
            Name = model.Name;
            KohatuKbn = model.KohatuKbn;
            MadokuKbn = model.MadokuKbn;
            KouseisinKbn = model.KouseisinKbn;
            OdrUnitName = model.OdrUnitName;
            EndDate = model.EndDate;
            DrugKbn = model.DrugKbn;
            MasterSbt = model.MasterSbt;
            BuiKbn = model.BuiKbn;
            IsAdopted = model.IsAdopted;
            Ten = model.Ten;
            TenId = model.TenId;
            KensaMstCenterItemCd1 = model.KensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = model.KensaMstCenterItemCd2;
            CmtCol1 = model.CmtCol1;
            IpnNameCd = model.IpnNameCd;
            SinKouiKbn = model.SinKouiKbn;
            YjCd = model.YjCd;
            CnvUnitName = model.CnvUnitName;
            StartDate = model.StartDate;
            YohoKbn = model.YohoKbn;
            CmtColKeta1 = model.CmtColKeta1;
            CmtColKeta2 = model.CmtColKeta2;
            CmtColKeta3 = model.CmtColKeta3;
            CmtColKeta4 = model.CmtColKeta4;
            CmtCol2 = model.CmtCol2;
            CmtCol3 = model.CmtCol3;
            CmtCol4 = model.CmtCol4;
            IpnCD = model.IpnCD;
            RousaiKbnDisplay = model.RousaiKbnDisplay;
            KouseisinKbnDisplay = model.KouseisinKbnDisplay;
            KubunToDisplay = model.KubunToDisplay;
            KohatuKbnDisplay = model.KohatuKbnDisplay;
            KensaCenterItemCDDisplay = model.KensaCenterItemCDDisplay;
            TenDisplay = model.TenDisplay;
            KouiName = model.KouiName;
            OdrTermVal = model.OdrTermVal;
            CnvTermVal = model.CnvTermVal;
            DefaultValue = model.DefaultValue;
        }

        [JsonConstructor]
        public TenItemDto(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCD, string rousaiKbnDisplay, string kouseisinKbnDisplay, string kubunToDisplay, string kohatuKbnDisplay, string kensaCenterItemCDDisplay, string tenDisplay, string kouiName, double odrTermVal, double cnvTermVal, double defaultValue, int modeStatus)
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
            IsAdopted = isAdopted;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
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
            IpnCD = ipnCD;
            RousaiKbnDisplay = rousaiKbnDisplay;
            KouseisinKbnDisplay = kouseisinKbnDisplay;
            KubunToDisplay = kubunToDisplay;
            KohatuKbnDisplay = kohatuKbnDisplay;
            KensaCenterItemCDDisplay = kensaCenterItemCDDisplay;
            TenDisplay = tenDisplay;
            KouiName = kouiName;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            ModeStatus = modeStatus;
        }

        public int HpId { get; private set; }

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

        public string RousaiKbnDisplay { get; private set; }

        public string KouseisinKbnDisplay { get; private set; }

        public string KubunToDisplay { get; private set; }

        public string KohatuKbnDisplay { get; private set; }

        public string KensaCenterItemCDDisplay { get; private set; }

        public string TenDisplay { get; private set; }

        public string KouiName { get; private set; }

        public double OdrTermVal { get; private set; }

        public double CnvTermVal { get; private set; }

        public double DefaultValue { get; private set; }

        public int ModeStatus { get; private set; }
    }
}

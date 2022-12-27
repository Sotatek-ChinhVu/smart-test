using Domain.Models.MstItem;

namespace UseCase.MstItem.GetAdoptedItemList
{
    public class TenMstItem
    {

        public TenMstItem(TenItemModel tenItemModel)
        {
            HpId = tenItemModel.HpId;
            ItemCd = tenItemModel.ItemCd;
            RousaiKbn = tenItemModel.RousaiKbn;
            KanaName1 = tenItemModel.KanaName1;
            Name = tenItemModel.Name;
            KohatuKbn = tenItemModel.KohatuKbn;
            MadokuKbn = tenItemModel.MadokuKbn;
            KouseisinKbn = tenItemModel.KouseisinKbn;
            OdrUnitName = tenItemModel.OdrUnitName;
            EndDate = tenItemModel.EndDate;
            DrugKbn = tenItemModel.DrugKbn;
            MasterSbt = tenItemModel.MasterSbt;
            BuiKbn = tenItemModel.BuiKbn;
            IsAdopted = tenItemModel.IsAdopted;
            Ten = tenItemModel.Ten;
            TenId = tenItemModel.TenId;
            CmtCol1 = tenItemModel.CmtCol1;
            IpnNameCd = tenItemModel.IpnNameCd;
            SinKouiKbn = tenItemModel.SinKouiKbn;
            YjCd = tenItemModel.YjCd;
            CnvUnitName = tenItemModel.CnvUnitName;
            StartDate = tenItemModel.StartDate;
            CmtCol4 = tenItemModel.CmtCol4;
            IpnCD = tenItemModel.IpnCD;
            MaxAge = tenItemModel.MaxAge;
            MinAge = tenItemModel.MinAge;
            SanteiItemCd = tenItemModel.SanteiItemCd;
        }

        public TenMstItem()
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
            IsAdopted = 0;
            Ten = 0;
            TenId = 0;
            CmtCol1 = 0;
            IpnNameCd = string.Empty;
            SinKouiKbn = 0;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            StartDate = 0;
            CmtCol4 = 0;
            IpnCD = string.Empty;
            MaxAge = string.Empty;
            MinAge = string.Empty;
            SanteiItemCd = string.Empty;
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

        public int CmtCol1 { get; private set; }

        public string IpnNameCd { get; private set; }

        public int SinKouiKbn { get; private set; }

        public string YjCd { get; private set; }

        public string CnvUnitName { get; private set; }

        public int StartDate { get; private set; }

        public int YohoKbn { get; private set; }

        public int CmtCol4 { get; private set; }

        public string IpnCD { get; private set; }

        public string MaxAge { get; private set; }

        public string MinAge { get; private set; }

        public string SanteiItemCd { get; private set; }
    }
}


using CommonChecker.Types;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace CommonChecker.Models.OrdInfDetailModel
{
    public class OrdInfoDetailModel : IOdrInfoDetailModel
    {
        public int Id { get; set; }
        public int SinKouiKbn { get; set; }
        public string ItemCd { get; set; }
        public string ItemName { get; set; }
        public double Suryo { get; set; }
        public string UnitName { get; set; }
        public double TermVal { get; set; }
        public int SyohoKbn { get; set; }
        public int SyohoLimitKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public string IpnCd { get; set; }
        public string Bunkatu { get; set; }
        public string MasterSbt { get; set; }
        public int BunkatuKoui { get; set; }




        public bool IsDrugUsage
        {
            get => YohoKbn > 0 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        }

        public bool IsDrug
        {
            get => (SinKouiKbn == 20 && DrugKbn > 0) || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
                || (SinKouiKbn == 20 && ItemCd.StartsWith("Z"));
        }

        public bool IsInjection
        {
            get => SinKouiKbn == 30;
        }

        public bool Is820Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment820Pattern);

        public bool Is830Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

        public bool Is831Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

        public bool Is850Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

        public bool Is851Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

        public bool Is852Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

        public bool Is853Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

        public bool Is840Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon;

        public bool Is842Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

        public bool Is880Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

        public bool IsShohoComment => SinKouiKbn == 100;

        public bool IsShohoBiko => SinKouiKbn == 101;

        public bool IsStandardUsage
        {
            get => YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        }

        public bool IsInjectionUsage
        {
            get => (SinKouiKbn >= 31 && SinKouiKbn <= 34) || (SinKouiKbn == 30 && ItemCd.StartsWith("Z") && MasterSbt == "S");
        }

        public bool IsSuppUsage
        {
            get => YohoKbn == 2;
        }

        public string DisplayedUnit
        {
            get => Suryo.AsDouble() != 0 ? UnitName.AsString() : "";
        }

        public string DisplayedQuantity
        {
            get
            {
                // If item don't have UniName => No quantity displayed
                if (string.IsNullOrEmpty(DisplayedUnit))
                {
                    return string.Empty;
                }
                return Suryo.AsDouble() != 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu ? Suryo.AsDouble().AsString() : "";
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ItemCd) &&
                       string.IsNullOrEmpty(ItemName.Trim()) &&
                       SinKouiKbn == 0;
            }
        }

        public string DisplayItemName
        {
            get
            {
                if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    return ItemName + TenUtils.GetBunkatu(BunkatuKoui, Bunkatu);
                }
                else if (Is840Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is842Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is830Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is831Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is850Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is851Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is852Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is853Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is880Cmt)
                {
                    return "" + ItemName;
                }
                else if (string.IsNullOrEmpty(ItemCd) && !IsShohoComment && !IsShohoBiko)
                {
                    return "" + ItemName;
                }
                return ItemName;
            }
        }

        public bool IsUsage
        {
            get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
        }

        public ReleasedDrugType ReleasedType
        {
            get => CIUtil.SyohoToSempatu(SyohoKbn, SyohoLimitKbn);
        }
    }
}

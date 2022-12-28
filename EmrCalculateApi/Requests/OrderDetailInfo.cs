using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace EmrCalculateApi.Requests
{
    public class OrderDetailInfo
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long RpNo { get; set; }

        public long RpEdaNo { get; set; }

        public int RowNo { get; set; }

        public int SinKouiKbn { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public double Suryo { get; set; }

        public string UnitName { get; set; } = string.Empty;

        public int UnitSBT { get; set; }

        public double TermVal { get; set; }

        public int KohatuKbn { get; set; }

        public int SyohoKbn { get; set; }

        public int SyohoLimitKbn { get; set; }

        public int DrugKbn { get; set; }

        public int YohoKbn { get; set; }

        public string Kokuji1 { get; set; } = string.Empty;

        public string Kokuji2 { get; set; } = string.Empty;

        public int IsNodspRece { get; set; }

        public string IpnCd { get; set; } = string.Empty;

        public string IpnName { get; set; } = string.Empty;

        public int JissiKbn { get; set; }

        public DateTime JissiDate { get; set; }

        public int JissiId { get; set; }

        public string JissiMachine { get; set; } = string.Empty;

        public string ReqCd { get; set; } = string.Empty;

        public string Bunkatu { get; set; } = string.Empty;

        public string CmtName { get; set; } = string.Empty;

        public string CmtOpt { get; set; } = string.Empty;

        public int CommentNewline { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string FontColor { get; set; } = string.Empty;


        #region Exposed properties

        public bool IsShownCheckbox { get; set; }

        public bool IsChecked { get; set; }

        public bool IsSelected { get; set; }

        public string DisplayedQuantity { get; set; } = string.Empty;

        public string DisplayedUnit
        {
            get => Suryo.AsDouble() != 0 ? UnitName.AsString() : "";
        }

        public int CmtCol1 { get; set; }

        public int CmtCol2 { get; set; }

        public int CmtCol3 { get; set; }

        public int CmtCol4 { get; set; }

        public int CmtColKeta1 { get; set; }

        public int CmtColKeta2 { get; set; }

        public int CmtColKeta3 { get; set; }

        public int CmtColKeta4 { get; set; }

        public bool IsKohatu
        {
            get => KohatuKbn == 1 || KohatuKbn == 2;
        }

        public KensaMstInfo KensaMstModel { get; set; }

        public double Ten { get; set; }

        public int HandanGrpKbn { get; set; }

        public string MasterSbt { get; set; } = string.Empty;

        public bool IsSpecialItem
        {
            get => MasterSbt == "S"
                && SinKouiKbn == 20
                && DrugKbn == 0
                && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu;
        }

        public IpnMinYakkaMstInfo IpnMinYakkaMstModel { get; set; }

        /// <summary>
        /// check gaichu only
        /// </summary>
        public bool IsKensa
        {
            get => SinKouiKbn == 61 || SinKouiKbn == 64;
        }

        #region From TodayOdrInf
        public int InOutKbn { get; set; }

        public int OdrInfOdrKouiKbn { get; set; }

        public bool IsInDrugOdr { get; set; }
        #endregion

        public bool IsStandardUsage
        {
            get => YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        }

        public bool IsSuppUsage
        {
            get => YohoKbn == 2;
        }

        public bool IsInjectionUsage
        {
            get => (SinKouiKbn >= 31 && SinKouiKbn <= 34) || (SinKouiKbn == 30 && ItemCd.StartsWith("Z") && MasterSbt == "S");
        }

        public bool IsHighlight { get; set; }

        public bool IsFocused { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ItemCd) &&
                       string.IsNullOrEmpty(ItemName?.Trim()) &&
                       SinKouiKbn == 0;
            }
        }

        public ModelStatus Status { get; private set; } = ModelStatus.None;

        public bool IsFreeComment => IsNormalComment || IsFree830Prefix || IsFree831Prefix || IsFree880Prefix;

        public bool IsNormalComment => !string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(ItemCd);

        public bool IsStartComment { get; set; }

        public bool IsComment
        {
            get => IsNormalComment || Is820Cmt || Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;
        }

        public bool IsFree830Prefix => Is830Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        public bool IsFree831Prefix => Is831Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        public bool IsFree880Prefix => Is880Cmt && OdrUtil.IsFree830Prefix(SearchingText, CmtName);

        public string SearchingText { get; set; } = string.Empty;

        public string DisplayFontColor { get; set; } = String.Empty;

        public bool IsUsage
        {
            get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
        }

        public int BunkatuKoui { get; set; }

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

        public bool IsJihi => ItemCd != null && ItemCd.StartsWith(ItemCdConst.ItemJihi);

        public bool IsShohoComment => SinKouiKbn == 100;

        public bool IsShohoBiko => SinKouiKbn == 101;

        public bool IsEditingQuantity { get; set; }

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

        public double BackupSuryo { get; set; }

        public int AlternationIndex { get; set; }

        /// <summary>
        /// Using to detect CmtOpt is changing
        /// </summary>
        public bool CmtOptChanging { get; }
        #endregion

        /// <summary>
        /// 算定漏れ確認用ダミー項目
        /// </summary>
        public bool IsDummy { get; set; } = false;
    }
}

using Helper.Constants;
using Reporting.CommonMasters.Models;

namespace Reporting.OrderLabel.Model;

public interface IOdrInfDetailModel
{
    int HpId { get; }

    long RpNo { get; }

    long RpEdaNo { get; }

    int RowNo { get; set; }

    int SinKouiKbn { get; set; }

    string ItemCd { get; set; }

    string ItemName { get; set; }

    string DisplayItemName { get; }

    double Suryo { get; set; }

    string UnitName { get; set; }

    int UnitSBT { get; set; }

    double TermVal { get; }

    int KohatuKbn { get; }

    int SyohoKbn { get; }

    int SyohoLimitKbn { get; }

    int DrugKbn { get; }

    int YohoKbn { get; }

    string Kokuji1 { get; }

    string Kokuji2 { get; }

    int IsNodspRece { get; }

    string IpnCd { get; }

    string IpnName { get; }

    string Bunkatu { get; }

    string CmtName { get; set; }

    string CmtOpt { get; set; }

    string FontColor { get; }

    #region Exposed properties
    bool IsEmpty { get; }

    bool IsShownCheckbox { get; }

    bool IsChecked { get; set; }

    bool IsSelected { get; set; }

    string DisplayedQuantity { get; }

    string DisplayedUnit { get; }

    string EditingQuantity { get; set; }

    int CmtCol1 { get; set; }

    int CmtCol2 { get; set; }

    int CmtCol3 { get; set; }

    int CmtCol4 { get; set; }

    int CmtColKeta1 { get; set; }

    int CmtColKeta2 { get; set; }

    int CmtColKeta3 { get; set; }

    int CmtColKeta4 { get; set; }

    ReleasedDrugType ReleasedType { get; }

    bool IsShownReleasedDrug { get; }

    bool IsKohatu { get; }

    KensaMstModel KensaMstModel { get; set; }

    /// <summary>
    /// check gaichu only
    /// </summary>
    bool IsKensa { get; }

    bool IsUsage { get; }

    int KensaGaichu { get; }

    /// <summary>
    /// 基本用法
    /// </summary>
    bool IsStandardUsage { get; }

    /// <summary>
    /// 補助用法
    /// </summary>
    bool IsSuppUsage { get; }

    bool IsInjectionUsage { get; }

    ModelStatus Status { get; }

    bool IsComment { get; }

    string DisplayFontColor { get; }

    string SearchingText { get; set; }

    int BunkatuKoui { get; set; }
    #endregion
}

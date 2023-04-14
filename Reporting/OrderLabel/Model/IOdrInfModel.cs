using Helper.Enum;

namespace Reporting.OrderLabel.Model
{
    public interface IOdrInfModel<TOdrInfDetailModel>
        where TOdrInfDetailModel : class, IOdrInfDetailModel
    {
        long RpNo { get; }

        long RpEdaNo { get; }

        int OdrKouiKbn { get; }

        string RpName { get; }

        int InoutKbn { get; }

        int SikyuKbn { get; }

        int SyohoSbt { get; }

        int SanteiKbn { get; }

        int TosekiKbn { get; }

        int DaysCnt { get; }

        int SortNo { get; set; }

        int GroupOdrKouiKbn { get; }

        int IsDeleted { get; set; }

        #region Exposed
        List<TOdrInfDetailModel> OdrInfDetailModels { get; set; }

        List<TOdrInfDetailModel> OdrInfDetailModelsIgnoreEmpty { get; }

        TOdrInfDetailModel SelectedOrderDetailModel { get; set; }

        bool IsShowTitle { get; }

        bool IsShowCollapsedTitle { get; }

        string OdrInfTitle { get; }

        // 処方 - Drug
        bool IsDrug { get; }

        // 注射 - Injection
        bool IsInjection { get; }

        string DrugPrice { get; }

        bool IsExpanded { get; set; }

        bool IsShownCheckbox { get; set; }

        bool IsChecked { get; set; }

        bool IsSelected { get; set; }

        bool IsVisible { get; }

        string GUID { get; }

        UsingDrugType UsingType { get; }

        bool IsShownQuantityColumn { get; set; }

        bool IsShownUnitColumn { get; set; }

        bool IsShownReleasedDrugColumn { get; set; }

        double ItemNameColumnWidth { get; set; }

        double QuantityColumnWidth { get; set; }

        double UnitColumnWidth { get; set; }

        double ReleasedDrugColumnWidth { get; set; }

        bool IsItemNameReadOnly { get; }

        bool IsQuantityReadOnly { get; }

        bool IsShowKensaGaichu { get; }

        bool IsAddNew { get; }
        #endregion
    }
}

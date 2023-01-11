using Helper.Constants;

namespace CommonChecker.Types
{
    public interface IOdrInfoDetailModel
    {
        int RowNo { get; }

        int RpNo { get; }

        int SinKouiKbn { get; }

        string ItemCd { get; }

        string ItemName { get; }

        double Suryo { get; }

        string UnitName { get; }

        double TermVal { get; }

        int SyohoKbn { get; }

        int SyohoLimitKbn { get; }

        int DrugKbn { get; }

        int YohoKbn { get; }

        string IpnCd { get; }

        string Bunkatu { get; }

        #region Exposed properties
        bool IsEmpty { get; }

        string DisplayedQuantity { get; }

        string DisplayedUnit { get; }

        string MasterSbt { get; }

        bool IsStandardUsage { get; }

        bool IsSuppUsage { get; }

        bool IsInjectionUsage { get; }

        bool IsDrugUsage { get; }

        bool IsDrug { get; }

        bool IsInjection { get; }

        bool Is830Cmt { get; }

        bool Is831Cmt { get; }

        bool Is840Cmt { get; }

        bool Is842Cmt { get; }

        bool Is850Cmt { get; }

        bool Is851Cmt { get; }

        bool Is852Cmt { get; }

        bool Is853Cmt { get; }

        bool Is880Cmt { get; }

        bool IsShohoBiko { get; }

        bool IsShohoComment { get; }
        #endregion
        string DisplayItemName { get; }

        bool IsUsage { get; }

        ReleasedDrugType ReleasedType { get; }
    }
}

namespace Domain.Types
{
    public interface IOdrInfDetailModel
    {
        int HpId { get; }

        long RpNo { get; }

        long RpEdaNo { get; }

        int RowNo { get; }

        int SinKouiKbn { get; }

        string ItemCd { get; }

        string ItemName { get; }
        string DisplayItemName { get; }

        double Suryo { get; }

        string UnitName { get; }

        int UnitSbt { get; }

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

        string CmtName { get; }

        string CmtOpt { get; }

        string FontColor { get; }

        int CommentNewline { get; }

        #region Exposed properties
        bool IsEmpty { get; }

        double Price { get; }

        string DisplayedQuantity { get; }

        string DisplayedUnit { get; }

        int CmtCol1 { get; }

        double Ten { get; }

        string MasterSbt { get; }
        bool IsUsage { get; }

        bool IsSpecialItem { get; }

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
    }
}

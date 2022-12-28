namespace EmrCalculateApi.Requests
{
    public class OrderInfo
    {
        public List<OrderDetailInfo> DetailInfoList { get; set; } = new List<OrderDetailInfo>();

        public long Id { get; set; }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long RpNo { get; set; }

        public long RpEdaNo { get; set; }

        public int HokenPid { get; set; }

        public int OdrKouiKbn { get; set; }

        public int GroupOdrKouiKbn { get; set; }

        public string RpName { get; set; } = string.Empty;

        public int InoutKbn { get; set; }

        public int SikyuKbn { get; set; }

        public int SyohoSbt { get; set; }

        public int SanteiKbn { get; set; }

        public int TosekiKbn { get; set; }

        public int DaysCnt { get; set; }

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateId { get; set; }

        public string CreateMachine { get; set; } = string.Empty;

        public DateTime UpdateDate { get; set; }

        public int UpdateId { get; set; }

        public string UpdateMachine { get; set; } = string.Empty;

        #region Exposed properties

        public bool IsVisible
        {
            get => IsDeleted == 0;
        }

        public bool IsAddNew
        {
            get => Id == 0;
        }

        public bool IsSelfInjection => OdrKouiKbn == 28;

        public bool IsDrug
        {
            get
            {
                return OdrKouiKbn >= 21 && OdrKouiKbn <= 23;
            }
        }

        public bool IsInjection
        {
            get
            {
                return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
            }
        }

        public bool IsShohoComment
        {
            get => OdrKouiKbn == 100;
        }

        public bool IsShohoBiko
        {
            get => OdrKouiKbn == 101;
        }

        public bool IsShohosenComment
        {
            get => IsShohoComment || IsShohoBiko;
        }

        public bool IsExpanded { get; set; }

        public bool IsShowCollapsedTitle
        {
            get => !this.IsExpanded;
        }

        public bool IsShownCheckbox { get; set; }

        public bool IsChecked { get; set; }

        public bool IsSelected { get; set; }

        public string GUID { get; } = Guid.NewGuid().ToString();
        public bool IsShownReleasedDrugColumn { get; set; }

        public bool IsShownQuantityColumn { get; set; }

        public bool IsShownUnitColumn { get; set; }

        public double ItemNameColumnWidth { get; set; }

        public double QuantityColumnWidth { get; set; }

        public double UnitColumnWidth { get; set; }

        public double ReleasedDrugColumnWidth { get; set; }

        public bool IsItemNameReadOnly => false;

        public bool IsQuantityReadOnly => false;

        public bool IsShowKensaGaichu => true;

        public bool IsSameGroupWith(OrderInfo other)
        {
            return this.HokenPid == other.HokenPid
                && this.GroupOdrKouiKbn == other.GroupOdrKouiKbn
                && this.InoutKbn == other.InoutKbn
                && this.SikyuKbn == other.SikyuKbn
                && this.TosekiKbn == other.TosekiKbn
                && this.SyohoSbt == other.SyohoSbt
                && this.SanteiKbn == other.SanteiKbn;
        }
        #endregion
    }
}

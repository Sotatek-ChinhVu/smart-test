using Helper.Enum;
using System.Text.Json.Serialization;

namespace UseCase.MstItem.SearchTenMstItemSpecialNote
{
    public class SearchItemCondition
    {
        [JsonConstructor]
        public SearchItemCondition(string keyword, double pointFrom, double pointTo, int kouiKbn, int oriKouiKbn, List<int> kouiKbns, bool includeRosai, bool includeMisai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage, bool onlyUsage, string yJCode, bool isMasterSearch, bool isExpiredSearchIfNoData, bool isAllowSearchDeletedItem, bool isExpired, bool isDeleted, List<int> drugKbns, bool isSearchSanteiItem, bool isSearchKenSaItem, List<ItemTypeEnums> itemFilter, bool isSearch831SuffixOnly)
        {
            Keyword = keyword;
            PointFrom = pointFrom;
            PointTo = pointTo;
            KouiKbn = kouiKbn;
            OriKouiKbn = oriKouiKbn;
            KouiKbns = kouiKbns;
            IncludeRosai = includeRosai;
            IncludeMisai = includeMisai;
            STDDate = sTDDate;
            ItemCodeStartWith = itemCodeStartWith;
            IsIncludeUsage = isIncludeUsage;
            OnlyUsage = onlyUsage;
            YJCode = yJCode;
            IsMasterSearch = isMasterSearch;
            IsExpiredSearchIfNoData = isExpiredSearchIfNoData;
            IsAllowSearchDeletedItem = isAllowSearchDeletedItem;
            IsExpired = isExpired;
            IsDeleted = isDeleted;
            DrugKbns = drugKbns;
            IsSearchSanteiItem = isSearchSanteiItem;
            IsSearchKenSaItem = isSearchKenSaItem;
            ItemFilter = itemFilter;
            IsSearch831SuffixOnly = isSearch831SuffixOnly;
        }

        public SearchItemCondition() { }

        public string Keyword { get; private set; } = string.Empty;
        public double PointFrom { get; private set; } = -1;
        public double PointTo { get; private set; } = -1;
        public int KouiKbn { get; private set; }
        public int OriKouiKbn { get; private set; }
        public List<int> KouiKbns { get; private set; } = new();
        public bool IncludeRosai { get; private set; }
        public bool IncludeMisai { get; private set; }
        public int STDDate { get; private set; }
        public string ItemCodeStartWith { get; private set; } = string.Empty;
        public bool IsIncludeUsage { get; private set; } = true;
        public bool OnlyUsage { get; private set; } = false;
        public string YJCode { get; private set; } = string.Empty;
        public bool IsMasterSearch { get; private set; } = false;
        public bool IsExpiredSearchIfNoData { get; private set; } = false;
        public bool IsAllowSearchDeletedItem { get; private set; } = false;
        public bool IsExpired { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;
        public List<int> DrugKbns { get; private set; } = new();
        public bool IsSearchSanteiItem { get; private set; } = false;
        public bool IsSearchKenSaItem { get; private set; } = false;
        public List<ItemTypeEnums> ItemFilter { get; private set; } = new List<ItemTypeEnums>();
        public bool IsSearch831SuffixOnly { get; private set; } = false;
    }
}

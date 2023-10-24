using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenMstItem
{
    public class SearchTenMstItemInputData : IInputData<SearchTenMstItemOutputData>
    {
        public SearchTenMstItemInputData(int hpId, int pageIndex, int pageCount, string keyword, double? pointFrom, double? pointTo, int kouiKbn, int oriKouiKbn, List<int> kouiKbns, bool includeRosai, bool includeMisai, int sTDDate, string itemCodeStartWith, bool isIncludeUsage, bool onlyUsage, string yJCode, bool isMasterSearch, bool isExpiredSearchIfNoData, bool isAllowSearchDeletedItem, bool isExpired, bool isDeleted, List<int> drugKbns, bool isSearchSanteiItem, bool isSearchKenSaItem, List<ItemTypeEnums> itemFilter, bool isSearch831SuffixOnly, bool isSearchSuggestion, bool isSearchGazoDensibaitaiHozon, FilterTenMstEnum sortCol, SortType sortType)
        {
            HpId = hpId;
            PageIndex = pageIndex;
            PageCount = pageCount;
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
            IsSearchSuggestion = isSearchSuggestion;
            IsSearchGazoDensibaitaiHozon = isSearchGazoDensibaitaiHozon;
            SortCol = sortCol;
            SortType = sortType;
        }

        public int HpId { get; private set; }
        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }
        public string Keyword { get; private set; }
        public double? PointFrom { get; private set; }
        public double? PointTo { get; private set; }
        public int KouiKbn { get; private set; }
        public int OriKouiKbn { get; private set; }
        public List<int> KouiKbns { get; private set; }
        public bool IncludeRosai { get; private set; }
        public bool IncludeMisai { get; private set; }
        public int STDDate { get; private set; }
        public string ItemCodeStartWith { get; private set; }
        public bool IsIncludeUsage { get; private set; }
        public bool OnlyUsage { get; private set; }
        public string YJCode { get; private set; }
        public bool IsMasterSearch { get; private set; }
        public bool IsExpiredSearchIfNoData { get; private set; }
        public bool IsAllowSearchDeletedItem { get; private set; }
        public bool IsExpired { get; private set; }
        public bool IsDeleted { get; private set; }
        public List<int> DrugKbns { get; private set; }
        public bool IsSearchSanteiItem { get; private set; }
        public bool IsSearchKenSaItem { get; private set; }
        public List<ItemTypeEnums> ItemFilter { get; private set; }
        public bool IsSearch831SuffixOnly { get; private set; }
        public bool IsSearchSuggestion { get; private set; }
        public bool IsSearchGazoDensibaitaiHozon { get; private set; }
        public FilterTenMstEnum SortCol { get; private set; }
        public SortType SortType { get; private set; }
    }
}

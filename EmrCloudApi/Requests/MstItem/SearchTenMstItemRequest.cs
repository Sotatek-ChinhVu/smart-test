using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class SearchTenMstItemRequest
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public double? PointFrom { get; set; } = null;
        public double? PointTo { get; set; } = null;
        public int KouiKbn { get; set; }
        public int OriKouiKbn { get; set; }
        public List<int> KouiKbns { get; set; } = new();
        public bool IncludeRosai { get; set; }
        public bool IncludeMisai { get; set; }
        public int SinDate { get; set; }
        public string ItemCodeStartWith { get; set; } = string.Empty;
        public bool IsIncludeUsage { get; set; } = true;
        public bool OnlyUsage { get; set; } = false;
        public string YJCode { get; set; } = string.Empty;
        public bool IsMasterSearch { get; set; } = false;
        public bool IsExpiredSearchIfNoData { get; set; } = false;
        public bool IsAllowSearchDeletedItem { get; set; } = false;
        public bool IsExpired { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public List<int> DrugKbns { get; set; } = new();
        public bool IsSearchSanteiItem { get; set; } = false;
        public bool IsSearchKenSaItem { get; set; } = false;
        public List<ItemTypeEnums> ItemFilter { get; set; } = new();
        public bool IsSearch831SuffixOnly { get; set; } = false;
        public bool IsSearchSuggestion { get; set; } = false;
        public bool IsSearchGazoDensibaitaiHozon { get; set; } = true;
        public FilterTenMstEnum SortCol { get; set; } = 0;
        public SortType SortType { get; set; } = 0;
    }
}
